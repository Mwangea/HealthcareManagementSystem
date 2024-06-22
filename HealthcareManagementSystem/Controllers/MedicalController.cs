using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;
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
            try
            {
                var medicalRecords = await _medicalService.GetMedicalRecordsByPatientIdAsync(patientId);
                return Ok(medicalRecords);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving medical records", error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            var result = await _medicalService.GetMedicalRecordByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            try
            {
                var medicalRecords = await _medicalService.GetAllMedicalRecordsAsync();
                return Ok(medicalRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving medical records", error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task <IActionResult> UpdateMedicalRecord(int id, [FromBody]UpdateMedicalRecordDTO updateMedicalRecord)
        {
            try
            {
                var medicalRecord = await _medicalService.UpdateMedicalRecordAsync(id, updateMedicalRecord);
                return Ok(medicalRecord);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task <IActionResult> DeleteMedicalRecord(int id)
        {
            var success = await _medicalService.DeleteMedicalRecordAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok(new { message = "deleted successfully" });
        }
    }
}
