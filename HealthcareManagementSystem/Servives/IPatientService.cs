using HealthcareManagementSystem.Data;

namespace HealthcareManagementSystem.Servives
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task AddPatientAsync(Patient patient);
        Task UpdatePatientAsync(int id);
        Task DeletePatientAsync(int id);
    }
}
