using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.AppointmentService
{
    public interface IAppointmentService
    {
        Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest createAppointmentRequest);
        Task<AppointmentResponse> GetAppointmentByIdAsync(int id);
    }
}
