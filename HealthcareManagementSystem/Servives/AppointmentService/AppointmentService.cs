using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest createAppointmentRequest)
        {
            var appointment = new Appointment
            {
                PatientId = createAppointmentRequest.PatientId,
                DoctorId = createAppointmentRequest.DoctorId,
                AppointmentDate = createAppointmentRequest.AppointmentDate,
                Status = createAppointmentRequest.Status,
                Notes = createAppointmentRequest.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };
        }

        public async Task<AppointmentResponse> GetAppointmentByIdAsync (int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return null;
            }

            return new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };
        }
    }
}
