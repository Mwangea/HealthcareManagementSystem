using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.DoctorService
{
    public interface IDoctorService
    {
        Task<DoctorLoginResponse> Authenticate(DoctorLoginRequest doctorLoginRequest);
        Task<DoctorRegisterResponse> Register(DoctorRegisterRequest doctorRegisterRequest);

        
    }
}
