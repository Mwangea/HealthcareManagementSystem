using HealthcareManagementSystem.DTOs;
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

        public MedicineController (IMedicineService mediicineService)
        {
            _medicineService = mediicineService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> AddMedicine([FromBody] CreateMedicineDTO createMedicine )
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
    }
}
