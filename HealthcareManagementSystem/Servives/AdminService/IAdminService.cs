using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.AdminService
{
    public interface IAdminService
    {
        Task<AdminLoginResponse> Authenticate(AdminLoginRequest adminLoginRequest);
        Task<AdminRegisterResponse> Register(AdminRegisterRequest adminRegisterRequest);

        Task DeleteDoctorAsync(int id);
    }
}
