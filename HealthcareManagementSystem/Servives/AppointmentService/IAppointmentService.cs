using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.AppointmentService
{
    public interface IAppointmentService
    {
        Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest createAppointmentRequest);
        Task<AppointmentResponse> GetAppointmentByIdAsync(int id);
        Task<List<AppointmentResponse>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<List<AppointmentResponse>> GetAllAppointmentsAsync();
        Task<AppointmentResponse> UpdateAppointmentAsync(int id, CreateAppointmentRequest updateAppointmentRequest);
        Task<bool> DeleteAppointmentAsync(int id);

    }
}
