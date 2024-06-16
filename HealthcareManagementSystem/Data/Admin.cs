using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.Data
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
       // public string Role { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
