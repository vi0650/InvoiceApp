using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace Invoice.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }

        public int RoleId { get; set; }
        [JsonIgnore]
        public RoleEntity Role { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
    }
}
