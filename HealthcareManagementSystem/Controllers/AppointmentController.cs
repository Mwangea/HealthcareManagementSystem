//using HealthcareManagementSystem.Services.AppointmentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Data;
using System.Security.Claims;
using HealthcareManagementSystem.Servives.AppointmentService;
using HealthcareManagementSystem.Servives.UserService;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]

        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _appointmentService.CreateAppointmentAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Doctor,Admin")]

        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var response = await _appointmentService.GetAppointmentByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Policy = "Doctor")]

        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId)
        {
            var response = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var response = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] CreateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _appointmentService.UpdateAppointmentAsync(id, request);
            if (response == null)
            {
                return NotFound(new { message = "Appoinment not found" });
            }

            return Ok(new { message = "Appointment updated successfully", response });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Deleted successfully" });
        }
    }
}
