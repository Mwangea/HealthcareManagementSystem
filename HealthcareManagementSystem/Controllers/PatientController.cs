using HealthcareManagementSystem.Servives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthcareManagementSystem.Data;

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
      //  [Authorize(Policy = "DoctorOnly")]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            return await _patientService.GetAllPatientsAsync();
        }

        [HttpGet("{id}")]
       // [Authorize(Policy = "DoctorOnly")]
        
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
        //[Authorize(Policy = "DoctorOnly")]
        public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        {
            await _patientService.AddPatientAsync(patient);

            return CreatedAtAction(
                nameof(GetPatientById),
                new { id = patient.Pat_id },
                new { message = "Patient created successfully", patient }
            );
        }

    }
}
