using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthcareManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("doctor/profile")]
        [Authorize(Policy = "Doctor")]
        public async Task<IActionResult> UpdateDoctorProfile([FromBody] DoctorProfileDto doctorProfileDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return Unauthorized();
            }

            var result = await _userService.UpdateDoctorProfileAsync(userId, doctorProfileDto);
            if (result)
            {
                return Ok(new { message = "Profile updated successfully" });
            }

            return BadRequest(new { message = "Failed to update profile" });
        }


        [HttpDelete("doctor/profile")]
        [Authorize(Policy = "Doctor")]
        public async Task<IActionResult> DeleteDoctorProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return Unauthorized();
            }

            var result = await _userService.DeleteDoctorProfileAsync(userId);
            if (result)
            {
                return Ok(new { message = "Account deleted successfully" });
            }

            return BadRequest(new { message = "Failed to delete account" });
        }

        [HttpPut("admin/profile")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateAdminProfile([FromBody] AdminProfileDto adminProfileDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return Unauthorized();
            }

            var result = await _userService.UpdateAdminProfileAsync(userId, adminProfileDto);
            if (result)
            {
                return Ok(new { message = "Profile updated successfully" });
            }

            return BadRequest(new { message = "Failed to update profile" });
        }

        [HttpDelete("admin/profile")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAdminProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return Unauthorized();
            }

            var result = await _userService.DeleteAdminProfileAsync(userId);
            if (result)
            {
                return Ok(new { message = "Account deleted successfully" });
            }

            return BadRequest(new { message = "Failed to delete account" });
        }
    }
}
