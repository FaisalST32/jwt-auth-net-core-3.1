using Auth.Application.Repositories;
using Auth.Application.ServiceInterfaces;
using Auth.Domain.Exceptions;
using Auth.Domain.User;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _userRepo;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;


        public AuthService(IUsersRepository userRepo, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }
        public async Task<LoginResponse> Login(string username, string password)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null)
            {
                throw new NotFoundException("cannot find user");
            }

            var canAuthenticate = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

            if (!canAuthenticate)
            {
                throw new InvalidCredentialsException("password is incorrect");
            }

            var claims = LoadClaims(user);

            var accessToken = GenerateAccessToken(claims);

            var isCurrentRefreshTokenInvalid = user.RefreshTokenExpiryUTC == null || user.RefreshTokenExpiryUTC < DateTime.UtcNow;

            if (isCurrentRefreshTokenInvalid)
            {
                user.RefreshToken = GenerateRefreshToken();
            }

            user.RefreshTokenExpiryUTC = DateTime.UtcNow.AddDays(30);

            await _userRepo.UpdateUser(user.Id, user);

            return new LoginResponse()
            {
                AuthToken = accessToken,
                RefreshToken = user.RefreshToken,
                UserDetails = user.UserProfile
            };
        }

        public async Task<LoginResponse> RefreshAuthToken(string refreshToken)
        {
            var user = await _userRepo.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                throw new NotFoundException("cannot find user");
            }

            if (user.RefreshTokenExpiryUTC < DateTime.UtcNow)
            {
                throw new InvalidCredentialsException("Refresh token has expired");
            }

            var claims = LoadClaims(user);

            var accessToken = GenerateAccessToken(claims);

            return new LoginResponse()
            {
                AuthToken = accessToken,
                RefreshToken = refreshToken,
                UserDetails = user.UserProfile
            };
        }

        private List<Claim> LoadClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserProfile.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.UserProfile.LastName)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("roles", role.Name));
            }

            return claims;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<bool> Logout(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> Register(string username, string password, UserProfile userInfo)
        {
            var user = new User()
            {
                Email = username,
                UserProfile = userInfo
            };
            user = PopulatePasswordDetais(password, user);

            return await _userRepo.AddUser(user);
        }

        private User PopulatePasswordDetais(string password, User user)
        {
            using (var hmac = new HMACSHA512())
            {
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                user.PasswordSalt = hmac.Key;
            }
            return user;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Auth:IssuerKey").Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _config.GetSection("Auth:Issuer").Value,
                audience: _config.GetSection("Auth:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Auth:IssuerKey").Value)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
