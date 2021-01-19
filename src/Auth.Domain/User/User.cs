using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain.User
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public DateTime RefreshTokenExpiryUTC { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}
