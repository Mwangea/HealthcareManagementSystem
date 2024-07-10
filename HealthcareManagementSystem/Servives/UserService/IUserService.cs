using System.Threading.Tasks;

namespace HealthcareManagementSystem.Servives.UserService
{
    public interface IUserService
    {
        //Task<int?> GetUserIdByUsernameAsync(string username);
        Task<int?> GetDoctorIdByUsernameAsync(string username);
        Task<int?> GetPatientIdByUsernameAsync(string username);
    }
}
