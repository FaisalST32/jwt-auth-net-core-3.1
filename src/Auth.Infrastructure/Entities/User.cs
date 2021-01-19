using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Infrastructure.Entities
{
    [Table("Users")]
    public class User : AuditableItem
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public DateTime RefreshTokenExpiryUTC { get; set; }
        public virtual List<UserRole> Roles { get; set; }
    }
}
