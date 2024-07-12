using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicalModel;
using HealthcareManagementSystem.Servives.AppointmentService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            if (patient == null)
                throw new ArgumentException($"Patient with username '{request.PatientUsername}' not found.");

            if (doctor == null)
                throw new ArgumentException($"Doctor with username '{request.DoctorUsername}' not found.");

            DateTime appointmentDateTime = request.GetAppointmentDateTime();

            var appointment = new Appointment
            {
                PatientId = patient.Pat_id,
                DoctorId = doctor.Id,
                AppointmentDate = appointmentDateTime,
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
                Date = appointmentDateTime.ToString("yyyy-MM-dd"),
                Time = appointmentDateTime.ToString("hh:mm tt", CultureInfo.InvariantCulture),
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
                return null;

            return new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Date = appointment.AppointmentDate.Date.ToString("yyyy-MM-dd"),
                Time = appointment.AppointmentDate.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                Status = appointment.Status,
                Notes = appointment.Notes,
                PatientName = appointment.Patient?.Username ?? "Unknown",
                DoctorName = appointment.Doctor?.Username ?? "Unknown"
            };
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
                    Date = a.AppointmentDate.Date.ToString("yyyy-MM-dd"),
                    Time = a.AppointmentDate.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    Status = a.Status,
                    Notes = a.Notes,
                    PatientName = a.Patient.Username ?? "Unknown",
                    DoctorName = a.Doctor.Username ?? "Unknown"
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
                    Date = a.AppointmentDate.Date.ToString("yyyy-MM-dd"),
                    Time = a.AppointmentDate.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    Status = a.Status,
                    Notes = a.Notes,
                    PatientName = a.Patient.Username ?? "Unknown",
                    DoctorName = a.Doctor.Username ?? "Unknown"
                }).ToListAsync();
        }

        public async Task<AppointmentResponse> UpdateAppointmentAsync(int id, CreateAppointmentRequest request)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                throw new ArgumentException($"Appointment with ID '{id}' not found.");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == request.PatientUsername);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username == request.DoctorUsername);

            if (patient == null)
                throw new ArgumentException($"Patient with username '{request.PatientUsername}' not found.");

            if (doctor == null)
                throw new ArgumentException($"Doctor with username '{request.DoctorUsername}' not found.");

            appointment.PatientId = patient.Pat_id;
            appointment.DoctorId = doctor.Id;
            appointment.AppointmentDate = request.GetAppointmentDateTime();
            appointment.Notes = request.Notes;

            await _context.SaveChangesAsync();

            return new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Date = appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                Time = appointment.AppointmentDate.ToString("hh:mm tt", CultureInfo.InvariantCulture),
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
                throw new ArgumentException($"Appointment with ID '{id}' not found.");

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
