using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.MedicalService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/medicalrecords")]
    [ApiController]
    public class MedicalController : ControllerBase
    {
        private readonly IMedicalService _medicalService;

        public MedicalController(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> AddMedicalRecord([FromBody] CreateMedicalRecordDTO createMedicalRecord)
        {
            
            try
            {
                var medicalRecord = await _medicalService.AddMedicalRecordAsync(createMedicalRecord);
                return Ok(new { message = "Medical Record Created successfully", medicalRecord });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Medical Record not created", error = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Policy ="Doctor,Admin")]
        public async Task<IActionResult> GetMedicalRecordsByPatientId(int patientId)
        {
            var medicalRecords = await _medicalService.GetMedicalRecordsByPatientIdAsync(patientId);
            return Ok(medicalRecords);
        }
    }
}
