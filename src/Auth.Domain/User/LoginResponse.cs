using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain.User
{
    public class LoginResponse
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public UserProfile UserDetails { get; set; }
    }
}
