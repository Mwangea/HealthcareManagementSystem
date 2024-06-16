using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Servives.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public DoctorService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<DoctorLoginResponse> Authenticate(DoctorLoginRequest doctorLoginRequest)
        {
            var doctor = await _context.Doctors
                .SingleOrDefaultAsync(a => a.Username == doctorLoginRequest.Username);

            if (doctor == null || !BCrypt.Net.BCrypt.Verify(doctorLoginRequest.Password, doctor.Password))
            {
                throw new Exception("Invalid credentials");
            }

            var token = GenerateJwtToken(doctor);

            return new DoctorLoginResponse { Token = token };
        }

        public async Task<DoctorRegisterResponse> Register(DoctorRegisterRequest doctorRegisterRequest)
        {
            if (string.IsNullOrEmpty(doctorRegisterRequest.Username) ||
                string.IsNullOrEmpty(doctorRegisterRequest.Password) ||
                string.IsNullOrEmpty(doctorRegisterRequest.Email) ||
                string.IsNullOrEmpty(doctorRegisterRequest.Specialty))
            {
                throw new ArgumentException("Username, password, email, and specialty are required");
            }

            if (await _context.Doctors.AnyAsync(d => d.Email == doctorRegisterRequest.Email))
            {
                throw new Exception("Email already exists");
            }

            var doctor = new Doctor
            {
                Username = doctorRegisterRequest.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(doctorRegisterRequest.Password),
                Email = doctorRegisterRequest.Email,
                Specialty = doctorRegisterRequest.Specialty
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return new DoctorRegisterResponse
            {
                Message = "Registration Successful",
                DoctorId = doctor.Id.ToString()
            };
        }

        private string GenerateJwtToken(Doctor doctor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, doctor.Id.ToString()),
                new Claim(ClaimTypes.Role, "Doctor")
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
