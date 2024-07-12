using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.DoctorService
{
    public interface IDoctorService
    {
        Task<DoctorLoginResponse> Authenticate(DoctorLoginRequest doctorLoginRequest);
        Task<DoctorRegisterResponse> Register(DoctorRegisterRequest doctorRegisterRequest);

        Task<DoctorDT0s> GetDoctorByIdAsync(int id);

        Task<DoctorDT0s> UpdateDoctorAsync(int id, DoctorUpdateRequest request);
        Task<bool> DeleteDoctorAsync(int id);


    }
}
