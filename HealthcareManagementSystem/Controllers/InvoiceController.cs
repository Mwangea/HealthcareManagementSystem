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
                var invoice = await _invoiceService.AddInvoiceAsync(invoiceDto);
                return Ok(new { message = "Invoice created successfully", invoice });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Invoice not created", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<InvoiceDTO>> GetInvoiceById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound(new { message = "Invoice not found" });
            }
            return Ok(invoice);
        }

        [HttpGet]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<ActionResult<List<InvoiceDTO>>> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> UpdateInvoice(int id, UpdateInvoiceDTO updateInvoiceDTO)
        {
            var updatedInvoice = await _invoiceService.UpdateInvoiceAsync(id, updateInvoiceDTO);
            {
                if (updatedInvoice == null)
                {
                    return NotFound();
                }

                return Ok(new { message = "Updated Successfully", updatedInvoice });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var deleted = await _invoiceService.DeleteInvoiceAsync(id);
            {
                if (deleted)
                {
                    return Ok(new { message = "Invoice deleted successfully" });
                }
                else
                {
                    return NotFound(new { message = " Invoice not found" });
                }
            }
        }
    } 
}
