using HealthcareManagementSystem.DTOs;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Services.UserService
{
    public interface IUserService
    {
        Task<DoctorProfileDto> GetDoctorProfileAsync(int userId);
        Task<AdminProfileDto> GetAdminProfileAsync(int userId);
        Task<bool> UpdateDoctorProfileAsync(int userId, DoctorProfileDto doctorProfileDto);
        Task<bool> UpdateAdminProfileAsync(int userId, AdminProfileDto adminProfileDto);
        Task<bool> DeleteDoctorProfileAsync(int userId);
        Task<bool> DeleteAdminProfileAsync(int userId);
    }
}
