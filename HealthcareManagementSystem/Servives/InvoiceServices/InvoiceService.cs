using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HealthcareManagementSystem.Servives.InvoiceServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> AddInvoiceAsync(CreateInvoiceDTO invoiceDto)
        {
            var invoice = new Invoice
            {
                PatientId = invoiceDto.PatientId,
                DoctorId = invoiceDto.DoctorId,
                Date = invoiceDto.Date,
                InvoiceNumber = GenerateInvoiceNumber(), 
                Subtotal = invoiceDto.Subtotal,
                Tax = invoiceDto.Tax,
                Total = invoiceDto.Total,
                
                Services = invoiceDto.Services.Select(s => new Service
            {

                    Description = s.Description,
                    Code = s.Code,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    Total = s.Quantity * s.UnitPrice,
                    
                }).ToList(),

                Charges = invoiceDto.Charges.Select(c => new Charge
                {
                    Description = c.Description,
                    Code = c.Code,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    Total = c.Quantity * c.UnitPrice,
                    
                }).ToList(),

                 PaymentMethod = invoiceDto.PaymentMethod,
                 PaymentDate = invoiceDto.PaymentDate,
                 AmountPaid = invoiceDto.AmountPaid
            };

            // Calculate subtotal
            invoice.Subtotal = invoice.Services.Sum(s => s.Total) + invoice.Charges.Sum(c => c.Total);

            // Calculate tax ( tax rate is 4%)
            decimal taxRate = 4; // 4% tax rate
            invoice.Tax = invoice.Subtotal * (taxRate / 100);

            // Calculate total
            invoice.Total = invoice.Subtotal + invoice.Tax;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }
        private string GenerateInvoiceNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var uniquePart = Guid.NewGuid().ToString().Split('-')[0]; // Use the first part of a GUID for uniqueness
            return $"INV-{datePart}-{uniquePart}";
        }
    }
}
