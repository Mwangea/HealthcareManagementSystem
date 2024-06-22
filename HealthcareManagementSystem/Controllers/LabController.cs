using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Servives.LaboratoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/labtests")]
    [ApiController]

    public class LabController : ControllerBase
    {
        private readonly ILaboratoryService _laboratoryService;

        public LabController(ILaboratoryService aboratoryService)
        {
            _laboratoryService = aboratoryService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> AddLabTest([FromBody] CreateLabTestDTO testDTO)
        {
            try
            {
                var labRecord = await _laboratoryService.AddLabTestAsync(testDTO);
                return Ok(new { message = "Medical Record Created successfully", labRecord });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Medical Record not created", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> GetLabTestById(int id)
        {
            try
            {
                var result = await _laboratoryService.GetAllLabTestByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "The specified lab test does not exist", error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> GetAllLabTest()
        {
            try
            {
                var labTest = await _laboratoryService.GetAllLabTestsAsync();
                return Ok(labTest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving labtest records", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy ="Doctor,Admin")]
        public async Task<IActionResult> UpdateLabTest(int id, [FromBody] UpdateLabTestDTO updateLabTestDTO)
        {
            try
            {
                var updatelabtest = await _laboratoryService.UpdateLabTestAsync(id, updateLabTestDTO);
                return Ok(new { message = "Lab Test Updated successfully", updatelabtest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lab Test not updated", error = ex.Message });


            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="Doctor,Admin")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var success = await _laboratoryService.DeleteLabTestAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok(new { message = "deleted successfully" });
        }
    }
}
