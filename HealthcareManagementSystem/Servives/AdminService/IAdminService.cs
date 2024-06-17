using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.AdminService
{
    public interface IAdminService
    {
        Task<AdminLoginResponse> Authenticate(AdminLoginRequest adminLoginRequest);
        Task<AdminRegisterResponse> Register(AdminRegisterRequest adminRegisterRequest);

        Task<List<DoctorDT0s>> GetAllDoctorsAsync();
        Task<DoctorDT0s> GetDoctorByIdAsync(int id);

        Task<DoctorDT0s> UpdateDoctorAsync(int id, DoctorUpdateRequest request);
        Task<bool> DeleteDoctorAsync(int id);
       

    }
}
