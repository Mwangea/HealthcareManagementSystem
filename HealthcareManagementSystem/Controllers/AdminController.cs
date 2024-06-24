using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthcareManagementSystem.Servives.DoctorService;
using Microsoft.AspNetCore.Authentication;

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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
         

            return Ok(new { message = "Logged out successfully" });
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

        [Authorize(Policy = "Admin")]
        [HttpPut("doctors/{id}")]

        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorUpdateRequest doctorUpdateRequest)
        {
            var doctor = await _adminService.UpdateDoctorAsync(id, doctorUpdateRequest);

            if (doctor == null)
            {
                return NotFound(new { message = "Doctor not found" });
            }

            var existingDoctor = await _adminService.GetDoctorByIdAsync(id);
            if (existingDoctor == null)
            {
                return NotFound(new { message = "Doctor not found" });
            }

           
            return Ok(new { message = "Doctor updated successfully" });
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("doctors/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var success = await _adminService.DeleteDoctorAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Doctor not found" });
            }

            return Ok(new { message = "Docotor deleted successfuly"});
        }

       

    }
}
