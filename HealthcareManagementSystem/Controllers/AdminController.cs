using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthcareManagementSystem.Servives.DoctorService;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/admin")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService; 

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AdminLoginResponse>> Login(AdminLoginRequest adminLoginRequest)
        {
            var response = await _adminService.Authenticate(adminLoginRequest);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AdminRegisterResponse>> Register(AdminRegisterRequest adminRegisterRequest)
        {
            var response = await _adminService.Register(adminRegisterRequest);
            return Ok(response);
        }

       

        [HttpGet("doctors")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<List<DoctorDT0s>>> GetAllDoctors()
        {
            var doctors = await _adminService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("doctors/{id}")]
        public async Task<ActionResult<DoctorDT0s>> GetDoctorById(int id)
        {
            var doctor = await _adminService.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }



    }
}
