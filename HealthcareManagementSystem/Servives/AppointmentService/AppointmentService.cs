using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<AppointmentResponse>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Notes = a.Notes
            }).ToListAsync();
        }

        public async Task<AppointmentResponse> UpdateAppointmentAsync(int id, CreateAppointmentRequest updateAppointmentRequest)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if(appointment == null)
            {
                return null;
            }

            appointment.PatientId = updateAppointmentRequest.PatientId;
            appointment.DoctorId = updateAppointmentRequest.DoctorId;
            appointment.AppointmentDate = updateAppointmentRequest.AppointmentDate;
            appointment.Status = updateAppointmentRequest.Status;
            appointment.Notes = updateAppointmentRequest.Notes;

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

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return false;
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
