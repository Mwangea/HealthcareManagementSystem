using HealthcareManagementSystem.Servives.AppointmentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Data;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<AppointmentResponse>> CreateAppointment(CreateAppointmentRequest createAppointmentRequest)
        {
            try
            {
                var response = await _appointmentService.CreateAppointmentAsync(createAppointmentRequest);
                return CreatedAtAction(nameof(GetAppointmentById),
                new { id = response.Id },
                new { message = "Appointment created successfully", response });
            }
            catch (Exception)
            {
               
                return StatusCode(500, new { message = "Doctor not found" });
            }

        }

        [HttpGet]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<List<AppointmentResponse>>> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<AppointmentResponse>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if(appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<AppointmentResponse>> UpdateAppointment(int id, CreateAppointmentRequest updateAppointmentRequest)
        {
            var updatedAppointment = await _appointmentService.UpdateAppointmentAsync(id, updateAppointmentRequest);

            if (updatedAppointment == null)
            {
                return NotFound();
            }

            return Ok( new { message = "updated successfully", updatedAppointment });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Doctor,Admin")]

        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var success = await _appointmentService.DeleteAppointmentAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok(new { message = "deleted successfully" });
        }
    }
}
