using HealthcareManagementSystem.DTOs;


namespace HealthcareManagementSystem.Servives
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(LoginRequest loginRequest);
        Task<RegisterResponse> Register(RegisterRequest registerRequest);
    }
}
