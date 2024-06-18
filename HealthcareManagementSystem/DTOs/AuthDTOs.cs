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
        public string Role { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
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

    public class AdminLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminLoginResponse
    {
        public string Token { get; set; }
    }

    public class AdminRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }

    public class AdminRegisterResponse
    {
        public string Message { get; set; }
        public string AdminId { get; set; }
    }

    public class DoctorRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Specialty is required")]
        public string Specialty { get; set; }

    }
    public class DoctorRegisterResponse
    {
        public string Message { get; set; }
        public string DoctorId { get; set; }

    }

    public class DoctorLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DoctorLoginResponse
    {
        public string Token { get; set; } 

    }

    public class CreateAppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
