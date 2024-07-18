using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.UserService;
using HealthcareManagementSystem.Services.UserService;

namespace HealthcareManagementSystem.Servives.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateDoctorProfileAsync(int userId, DoctorProfileDto doctorProfileDto)
        {
            var doctor = await _context.Doctors.FindAsync(userId);
            if (doctor == null)
                return false;

            // Update doctor properties
            doctor.Username = doctorProfileDto.Username;
            doctor.Email = doctorProfileDto.Email;
            doctor.Specialty = doctorProfileDto.Specialty;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDoctorProfileAsync(int userId)
        {
            var doctor = await _context.Doctors.FindAsync(userId);
            if (doctor == null)
                return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAdminProfileAsync(int userId, AdminProfileDto adminProfileDto)
        {
            var admin = await _context.Admins.FindAsync(userId);
            if (admin == null)
                return false;

            admin.Username = adminProfileDto.Username;
            admin.Email = adminProfileDto.Email;

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAdminProfileAsync(int userId)
        {
            var admin = await _context.Admins.FindAsync(userId);
            if (admin == null)
                return false;

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DoctorProfileDto> GetDoctorProfileAsync(int userId)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == userId);
            if (doctor == null)
                return null;

            return new DoctorProfileDto
            {
                Username = doctor.Username,
                Email = doctor.Email,
                Specialty = doctor.Specialty
            };
        }

        public async Task<AdminProfileDto> GetAdminProfileAsync(int userId)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == userId);
            if (admin == null)
                return null;

            return new AdminProfileDto
            {
                Username = admin.Username,
                Email = admin.Email
            };
        }
    }
}
