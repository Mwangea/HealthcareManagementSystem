using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicalModel;
using HealthcareManagementSystem.Servives.AppointmentService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == request.PatientUsername);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username == request.DoctorUsername);

            if (patient == null || doctor == null)
            {
                throw new ArgumentException("Invalid patient or doctor username");
            }

            var appointment = new Appointment
            {
                PatientId = patient.Pat_id,
                DoctorId = doctor.Id,
                AppointmentDate = request.AppointmentDate,
                Status = "Pending",
                Notes = request.Notes
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
                Notes = appointment.Notes,
                PatientName = patient.Username,
                DoctorName = doctor.Username
            };
        }

        public async Task<AppointmentResponse> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return null;  // Return null if appointment with the given id is not found
            }

            // Create an AppointmentResponse object with the retrieved data
            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            // Check if Patient and Doctor are not null before accessing their properties
            if (appointment.Patient != null)
            {
                response.PatientName = appointment.Patient.Username;
            }
            else
            {
                response.PatientName = "Unknown";  // Handle case where Patient is null
            }

            if (appointment.Doctor != null)
            {
                response.DoctorName = appointment.Doctor.Username;
            }
            else
            {
                response.DoctorName = "Unknown";  // Handle case where Doctor is null
            }

            return response;
        }



        public async Task<List<AppointmentResponse>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    Notes = a.Notes,
                    PatientName = a.Patient != null ? a.Patient.Username : "Unknown",
                    DoctorName = a.Doctor != null ? a.Doctor.Username : "Unknown"
                }).ToListAsync();
        }

        public async Task<List<AppointmentResponse>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    Notes = a.Notes,
                    PatientName = a.Patient != null ? a.Patient.Username : "Unknown",
                    DoctorName = a.Doctor != null ? a.Doctor.Username : "Unknown"
                }).ToListAsync();
        }

        public async Task<AppointmentResponse> UpdateAppointmentAsync(int id, CreateAppointmentRequest request)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return null;
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == request.PatientUsername);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username == request.DoctorUsername);

            if (patient == null || doctor == null)
            {
                throw new ArgumentException("Invalid patient or doctor username");
            }

            appointment.PatientId = patient.Pat_id;
            appointment.DoctorId = doctor.Id;
            appointment.AppointmentDate = request.AppointmentDate;
            appointment.Notes = request.Notes;

            await _context.SaveChangesAsync();

            return new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes,
                PatientName = patient.Username,
                DoctorName = doctor.Username
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
