using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.DoctorService;
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

    }
}
