using HealthcareManagementSystem.Servives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthcareManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            return await _patientService.GetAllPatientsAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        {
            await _patientService.AddPatientAsync(patient);

            return CreatedAtAction(
                nameof(GetPatientById),
                new { id = patient.Pat_id },
                new { message = "Patient created successfully", patient }
            );
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
        {
            if (id != patient.Pat_id)
            {
                return BadRequest(new { message = "Patient ID mismatch" });
            }

            var existingPatient = await _patientService.GetPatientByIdAsync(id);
            if (existingPatient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }

            await _patientService.UpdatePatientAsync(id);
            return Ok(new { message = "Patient updated successfully"});
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="Doctor,Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        { 
            await _patientService.DeletePatientAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }

    }
}
