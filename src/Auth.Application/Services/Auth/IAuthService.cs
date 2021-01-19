using Auth.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(string username, string password);
        Task<bool> Logout(string username);
        Task<UserProfile> Register(string username, string password, UserProfile userInfo);
        Task<LoginResponse> RefreshAuthToken(string refreshToken);
    }
}
