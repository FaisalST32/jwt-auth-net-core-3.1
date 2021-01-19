using Auth.API.ViewModels.Auth;
using Auth.Application.ServiceInterfaces;
using Auth.Domain.Exceptions;
using Auth.Domain.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var userDetails = await _authService.Login(login.Username, login.Password);

                return Ok(userDetails);
            }
            catch (NotFoundException)
            {
                return Unauthorized();
            }
            catch (InvalidCredentialsException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userInfo = new UserProfile
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email
            };

            var newUser = await _authService.Register(register.Email, register.Password, userInfo);
            return Ok(newUser);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAuthToken(RefreshTokenDto refreshToken)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken.RefreshToken))
                {
                    return BadRequest();
                }

                var userDetails = await _authService.RefreshAuthToken(refreshToken.RefreshToken);

                return Ok(userDetails);
            }
            catch (NotFoundException)
            {
                return BadRequest("refresh token is invalid");
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest("refresh token has expired");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
