using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicineModel;
using HealthcareManagementSystem.Servives.MedicineService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
//using Microsoft.AspNetCore.Mvc;


namespace HealthcareManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService mediicineService)
        {
            _medicineService = mediicineService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> AddMedicine([FromBody] CreateMedicineDTO createMedicine)
        {
            try
            {
                var medicine = await _medicineService.AddMedicineAsync(createMedicine);
                return Ok(new { message = "Medicine added successfully", medicine });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Medicine  not Added", error = ex.Message });
            }
        }

        [HttpPut("{id}/addstock")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddStock(int id, [FromBody] int quantity)
        {
            var medicine = await _medicineService.AddStockAsync(id, quantity);
            return Ok(medicine);
        }

        [HttpGet]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> GetAllMedicines()
        {
            var medicines = await _medicineService.GetAllMedicineAsync();
            return Ok(medicines);
        }

        [Authorize(Policy = "Doctor,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDTO>> GetMedicineById(int id)
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return Ok(medicine);
        }

        [HttpPut("{id}/buy")]

        public async Task<IActionResult> BuyMedicine(int id, [FromBody] int quantity)
        {
            try
            {
                var medicine = await _medicineService.BuyMedicineAsync(id, quantity);
                return Ok(medicine);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> UpdateMedicine(int id, [FromBody] UpdateMedicineDTO medicinedto)
        {


            var updatedMedicine = await _medicineService.UpdateMedicineAsync(id, medicinedto);

            if (updatedMedicine == null)
            {
                return NotFound(new { message = "Medicine not found" });
            }

            return Ok(new { message = "Medicine updated successfully", updatedMedicine });


        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Doctor,Admin")]

        public async Task<IActionResult> DeleteMedicine(int id)
        {
            await _medicineService.DeleteMedicineAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
    }
}
