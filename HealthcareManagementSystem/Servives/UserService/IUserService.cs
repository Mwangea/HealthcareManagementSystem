using HealthcareManagementSystem.DTOs;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Servives.UserService
{
    public interface IUserService
    {
        //Task<int?> GetUserIdByUsernameAsync(string username);
        Task<int?> GetDoctorIdByUsernameAsync(string username);
        Task<int?> GetPatientIdByUsernameAsync(string username);

        Task<bool> UpdateDoctorProfileAsync(int userId, DoctorProfileDto doctorProfileDto);
        Task<bool> DeleteDoctorProfileAsync(int userId);

        Task<bool> UpdateAdminProfileAsync(int userId, AdminProfileDto adminProfileDto);
        Task<bool> DeleteAdminProfileAsync(int userId);

    }
}
