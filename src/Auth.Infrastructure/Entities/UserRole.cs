using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Infrastructure.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
