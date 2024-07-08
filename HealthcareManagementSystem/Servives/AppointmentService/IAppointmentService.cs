using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.AppointmentService
{
    public interface IAppointmentService
    {
        Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest createAppointmentRequest);
        Task<List<AppointmentResponse>> GetAllAppointmentsAsync();
        Task<AppointmentResponse> GetAppointmentByIdAsync(int id);
        Task<AppointmentResponse> UpdateAppointmentAsync(int id, CreateAppointmentRequest updateAppointmentRequest);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<List<AppointmentResponse>> GetAppointmentsByDoctorAsync(int doctorId);

    }
}
