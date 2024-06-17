using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Servives.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AdminService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AdminLoginResponse> Authenticate(AdminLoginRequest adminLoginRequest)
        {
            var admin = await _context.Admins
                .SingleOrDefaultAsync(a => a.Username == adminLoginRequest.Username);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(adminLoginRequest.Password, admin.Password))
            {
                throw new Exception("Invalid credentials");
            }

            var token = GenerateJwtToken(admin);

            return new AdminLoginResponse { Token = token };
        }

        public async Task<AdminRegisterResponse> Register(AdminRegisterRequest adminRegisterRequest)
        {
            if (string.IsNullOrEmpty(adminRegisterRequest.Username) ||
                string.IsNullOrEmpty(adminRegisterRequest.Password) ||
                string.IsNullOrEmpty(adminRegisterRequest.Email))
            {
                throw new ArgumentException("Username, password, and email are required");
            }

            if (await _context.Admins.AnyAsync(a => a.Email == adminRegisterRequest.Email))
            {
                throw new Exception("Email already exists");
            }

            var admin = new Admin
            {
                Username = adminRegisterRequest.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(adminRegisterRequest.Password),
                Email = adminRegisterRequest.Email
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return new AdminRegisterResponse
            {
                Message = "Registration Successful",
                AdminId = admin.Id.ToString()
            };
        }

        public async Task<List<DoctorDT0s>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Select(d => new DoctorDT0s
                {
                    Id = d.Id,
                    Username = d.Username,
                    Email = d.Email,
                    Specialty = d.Specialty
                })
                .ToListAsync();
        }

        public async Task<DoctorDT0s> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return null;

            return new DoctorDT0s
            {
                Id = doctor.Id,
                Username = doctor.Username,
                Email = doctor.Email,
                Specialty = doctor.Specialty
            };
        }

        public async Task<DoctorDT0s> UpdateDoctorAsync(int id, DoctorUpdateRequest request)
        {

            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return null;

            doctor.Username = request.Username;
            doctor.Email = request.Email;
            doctor.Specialty = request.Specialty;
           

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return new DoctorDT0s
            {
                Id = doctor.Id,
                Username = doctor.Username,
                Email = doctor.Email,
                Specialty = doctor.Specialty,
                
            };

        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return true;
        }


        private string GenerateJwtToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        

      

    }
}
