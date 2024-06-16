using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _adminService.DeleteDoctorAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }



    }
}
