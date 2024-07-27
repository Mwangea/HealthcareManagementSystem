using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Helpers;
using HealthcareManagementSystem.Models.Invoicemodel;
using HealthcareManagementSystem.Services.InvoiceServices;
using HealthcareManagementSystem.Servives.InvoiceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<InvoiceResponse>> CreateInvoice([FromBody] CreateInvoiceDTO invoiceDto)
        {
            try
            {
                var invoiceResponse = await _invoiceService.AddInvoiceAsync(invoiceDto);
                return Ok(new { message = "Invoice created successfully", invoiceResponse });
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

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<List<InvoiceDTO>>> GetInvoicesByDoctorIdAsync(int doctorId)
        {
            try
            {
                var invoices = await _invoiceService.GetInvoicesByDoctorIdAsync(doctorId);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
            if (updatedInvoice == null)
            {
                return NotFound();
            }
            return Ok(new { message = "Updated Successfully", updatedInvoice });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Doctor,Admin")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var deleted = await _invoiceService.DeleteInvoiceAsync(id);
            if (deleted)
            {
                return Ok(new { message = "Invoice deleted successfully" });
            }
            else
            {
                return NotFound(new { message = "Invoice not found" });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            var invoiceDto = await _invoiceService.GetInvoiceByIdAsync(id);

            if (invoiceDto == null)
            {
                return NotFound();
            }

            var invoiceResponse = ConvertToInvoiceResponse(invoiceDto);

            var pdfBytes = PdfHelper.GenerateInvoicePdf(invoiceResponse);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoiceDto.Id}.pdf");
        }

        private InvoiceResponse ConvertToInvoiceResponse(InvoiceDTO invoiceDto)
        {
            var invoice = new Invoice
            {
                Invoice_id = invoiceDto.Id,
                PatientId = invoiceDto.PatientId,
                DoctorId = invoiceDto.DoctorId,
                Date = DateTime.ParseExact(invoiceDto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                InvoiceNumber = invoiceDto.InvoiceNumber,
                Subtotal = invoiceDto.Subtotal,
                Tax = invoiceDto.Tax,
                Total = invoiceDto.Total,
                PaymentMethod = invoiceDto.PaymentMethod,
                PaymentDate = DateTime.ParseExact(invoiceDto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                AmountPaid = invoiceDto.AmountPaid,
                Services = invoiceDto.Services.Select(s => new Service
                {
                    Description = s.Description,
                    Code = s.Code,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    Total = s.Total
                }).ToList(),
                Charges = invoiceDto.Charges.Select(c => new Charge
                {
                    Description = c.Description,
                    Code = c.Code,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    Total = c.Total
                }).ToList()
            };

            return new InvoiceResponse
            {
                Invoice = invoice,
                PatientUsername = invoiceDto.PatientName,
                DoctorUsername = invoiceDto.DoctorUsername
            };
        }
    }
}
