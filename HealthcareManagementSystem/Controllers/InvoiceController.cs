using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;
using HealthcareManagementSystem.Servives.InvoiceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase

    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Authorize(Policy = "Doctor,Admin")]
       public async Task<ActionResult<Invoice>> CreateInvoice([FromBody] CreateInvoiceDTO invoiceDto)
        {
            try
            {
               var invoice =  await _invoiceService.AddInvoiceAsync(invoiceDto);
                return Ok(new { message = "Invoice created successfully", invoice });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return StatusCode(500, new { message = "Invoice not created", error = ex.Message });
            }
        }
    }
}
