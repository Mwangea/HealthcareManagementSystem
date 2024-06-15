using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.Servives;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System;
using HealthcareManagementSystem.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest loginRequest)
        {
            // Retrieve user from the database
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new Exception("Invalid Credentials");
            }

            // Generate JWT Token
            var token = GenerateJwtToken(user);

            return new LoginResponse { Token = token, Role = user.Role };
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {

            //validate request
            if (string.IsNullOrEmpty(registerRequest.Username) ||
                string.IsNullOrEmpty(registerRequest.Password) ||
                string.IsNullOrEmpty(registerRequest.Email) ||
                string.IsNullOrEmpty(registerRequest.Role))
            {
                throw new ArgumentException("Username, password, email, and role are required");
            }
            if (await _context.Users.AnyAsync(u => u.Username == registerRequest.Username))
            {
                throw new Exception("Username already exists");
            }

            var user = new User
            {
                Username = registerRequest.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                Email = registerRequest.Email,
                Role = registerRequest.Role

            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new RegisterResponse
            {
                Message = "Registration successfull",
                UserId = user.Id.ToString()
            };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
                // You can add more claims here as needed
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
