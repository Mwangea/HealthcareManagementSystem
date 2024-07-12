using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<DoctorLoginResponse>> Login(DoctorLoginRequest doctorLoginRequest)
        {
            var response = await _doctorService.Authenticate(doctorLoginRequest);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<DoctorRegisterResponse>> Register(DoctorRegisterRequest doctorRegisterRequest)
        {
            var response = await _doctorService.Register(doctorRegisterRequest);
            return Ok(response);
        }

        [Authorize(Policy = "Doctor,Admin")]
        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorUpdateRequest doctorUpdateRequest)
        {
            var doctor = await _doctorService.UpdateDoctorAsync(id, doctorUpdateRequest);

            if (doctor == null)
            {
                return NotFound(new { message = "Doctor not found" });
            }

            var existingDoctor = await _doctorService.GetDoctorByIdAsync(id);
            if (existingDoctor == null)
            {
                return NotFound(new { message = "Doctor not found" });
            }


            return Ok(new { message = "Doctor updated successfully" });
        }

        [Authorize(Policy = "Doctor,Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var success = await _doctorService.DeleteDoctorAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Doctor not found" });
            }

            return Ok(new { message = "Docotor deleted successfuly" });
        }

    }
}
