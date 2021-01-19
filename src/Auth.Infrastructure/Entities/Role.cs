using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Infrastructure.Entities
{
    [Table("Roles")]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
