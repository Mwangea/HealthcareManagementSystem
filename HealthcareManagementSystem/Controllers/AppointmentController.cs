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
            var response = await _appointmentService.CreateAppointmentAsync(createAppointmentRequest);
            return CreatedAtAction(nameof(GetAppointmentById),
            new { id = response.Id },
            new { message = "Appointment created successfully", response });
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
    }
}
