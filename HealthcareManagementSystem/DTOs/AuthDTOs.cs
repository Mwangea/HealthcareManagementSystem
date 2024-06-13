using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.DTOs
{
    // DTOs for Authentication

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class RegisterResponse
    {
        public string Message { get; set; }
        public string UserId { get; set; }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public string Role { get; set; }
        
    }
}
