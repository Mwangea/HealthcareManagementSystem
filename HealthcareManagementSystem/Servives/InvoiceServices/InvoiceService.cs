using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;
using HealthcareManagementSystem.Services.InvoiceServices;
using System.Globalization;
using HealthcareManagementSystem.Servives.InvoiceServices;

namespace HealthcareManagementSystem.Services.InvoiceServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceResponse> AddInvoiceAsync(CreateInvoiceDTO invoiceDto)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == invoiceDto.PatientUsername);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username == invoiceDto.DoctorUsername);

            if (patient == null || doctor == null)
            {
                throw new InvalidOperationException("The specified Patient or Doctor does not exist.");
            }

            var invoice = new Invoice
            {
                PatientId = patient.Pat_id,
                DoctorId = doctor.Id,
                Date = DateTime.ParseExact(invoiceDto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                    Total = s.Quantity * s.UnitPrice
                }).ToList(),
                Charges = invoiceDto.Charges.Select(c => new Charge
                {
                    Description = c.Description,
                    Code = c.Code,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    Total = c.Quantity * c.UnitPrice
                }).ToList(),
                PaymentMethod = invoiceDto.PaymentMethod,
                PaymentDate = DateTime.ParseExact(invoiceDto.PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                AmountPaid = invoiceDto.AmountPaid
            };

            invoice.Subtotal = invoice.Services.Sum(s => s.Total) + invoice.Charges.Sum(c => c.Total);
            decimal taxRate = 4; // 4% tax rate
            invoice.Tax = invoice.Subtotal * (taxRate / 100);
            invoice.Total = invoice.Subtotal + invoice.Tax;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return new InvoiceResponse
            {
                Invoice = invoice,
                DoctorUsername = doctor.Username,
                PatientUsername = patient.Username,

            };
        }

        private string GenerateInvoiceNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var uniquePart = Guid.NewGuid().ToString().Split('-')[0]; // Use the first part of a GUID for uniqueness
            return $"INV-{datePart}-{uniquePart}";
        }

        public async Task<InvoiceDTO> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Services)
                .Include(i => i.Charges)
                .FirstOrDefaultAsync(i => i.Invoice_id == id);

            if (invoice == null)
            {
                return null;
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Pat_id == invoice.PatientId);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == invoice.DoctorId);

            if (patient == null || doctor == null)
            {
                return null;
            }

            return new InvoiceDTO
            {
                Id = invoice.Invoice_id,
                PatientId = invoice.PatientId,
                DoctorId = invoice.DoctorId,
                PatientName = patient.Username,
                DoctorUsername = doctor.Username,
                Date = invoice.Date.ToString("dd/MM/yyyy"),
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = invoice.Subtotal,
                Tax = invoice.Tax,
                Total = invoice.Total,
                PaymentMethod = invoice.PaymentMethod,
                PaymentDate = invoice.PaymentDate.ToString("dd/MM/yyyy"),
                AmountPaid = invoice.AmountPaid,
                Services = invoice.Services.Select(s => new ServiceDTO
                {
                    Id = s.Service_id,
                    Description = s.Description,
                    Code = s.Code,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    Total = s.Total
                }).ToList(),
                Charges = invoice.Charges.Select(c => new ChargeDTO
                {
                    Id = c.Charge_id,
                    Description = c.Description,
                    Code = c.Code,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    Total = c.Total
                }).ToList()
            };
        }
        public async Task<List<InvoiceDTO>> GetInvoicesByDoctorIdAsync(int doctorId)
        {
            var invoices = await _context.Invoices
                .Where(i => i.DoctorId == doctorId) // Compare using int
                .Include(i => i.Services)
                .Include(i => i.Charges)
                .ToListAsync();

            var invoiceDTOs = new List<InvoiceDTO>();

            foreach (var invoice in invoices)
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Pat_id == invoice.PatientId);
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == invoice.DoctorId);

                if (patient == null || doctor == null)
                {
                    continue; // Skip if patient or doctor is not found
                }

                var invoiceDTO = new InvoiceDTO
                {
                    Id = invoice.Invoice_id,
                    PatientId = invoice.PatientId,
                    DoctorId = invoice.DoctorId,
                    PatientName = patient.Username,
                    DoctorUsername = doctor.Username,
                    Date = invoice.Date.ToString("dd/MM/yyyy"),
                    InvoiceNumber = invoice.InvoiceNumber,
                    Subtotal = invoice.Subtotal,
                    Tax = invoice.Tax,
                    Total = invoice.Total,
                    PaymentMethod = invoice.PaymentMethod,
                    PaymentDate = invoice.PaymentDate.ToString("dd/MM/yyyy"),
                    AmountPaid = invoice.AmountPaid,
                    Services = invoice.Services.Select(s => new ServiceDTO
                    {
                        Id = s.Service_id,
                        Description = s.Description,
                        Code = s.Code,
                        Quantity = s.Quantity,
                        UnitPrice = s.UnitPrice,
                        Total = s.Total
                    }).ToList(),
                    Charges = invoice.Charges.Select(c => new ChargeDTO
                    {
                        Id = c.Charge_id,
                        Description = c.Description,
                        Code = c.Code,
                        Quantity = c.Quantity,
                        UnitPrice = c.UnitPrice,
                        Total = c.Total
                    }).ToList()
                };

                invoiceDTOs.Add(invoiceDTO);
            }

            return invoiceDTOs;
        }



        public async Task<List<InvoiceDTO>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Services)
                .Include(i => i.Charges)
                .ToListAsync();

            var invoiceDTOs = new List<InvoiceDTO>();

            foreach (var invoice in invoices)
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Pat_id == invoice.PatientId);
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == invoice.DoctorId);

                if (patient == null || doctor == null)
                {
                    continue; // Skip this invoice if patient or doctor is not found
                }

                var invoiceDTO = new InvoiceDTO
                {
                    Id = invoice.Invoice_id,
                    PatientId = invoice.PatientId,
                    DoctorId = invoice.DoctorId,
                    PatientName = patient.Username,
                    DoctorUsername = doctor.Username,
                    Date = invoice.Date.ToString("dd/MM/yyyy"),
                    InvoiceNumber = invoice.InvoiceNumber,
                    Subtotal = invoice.Subtotal,
                    Tax = invoice.Tax,
                    Total = invoice.Total,
                    PaymentMethod = invoice.PaymentMethod,
                    PaymentDate = invoice.PaymentDate.ToString("dd/MM/yyyy"),
                    AmountPaid = invoice.AmountPaid,
                    Services = invoice.Services.Select(s => new ServiceDTO
                    {
                        Id = s.Service_id,
                        Description = s.Description,
                        Code = s.Code,
                        Quantity = s.Quantity,
                        UnitPrice = s.UnitPrice,
                        Total = s.Total
                    }).ToList(),
                    Charges = invoice.Charges.Select(c => new ChargeDTO
                    {
                        Id = c.Charge_id,
                        Description = c.Description,
                        Code = c.Code,
                        Quantity = c.Quantity,
                        UnitPrice = c.UnitPrice,
                        Total = c.Total
                    }).ToList()
                };

                invoiceDTOs.Add(invoiceDTO);
            }

            return invoiceDTOs;
        }

        public async Task<InvoiceDTO> UpdateInvoiceAsync(int id, UpdateInvoiceDTO invoiceDto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Services)
                .Include(i => i.Charges)
                .FirstOrDefaultAsync(i => i.Invoice_id == id);

            if (invoice == null)
            {
                return null;
            }

            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == invoiceDto.DoctorId);
            if (!doctorExists)
            {
                throw new InvalidOperationException("The specified Doctor does not exist.");
            }

            var patientExists = await _context.Patients.AnyAsync(d => d.Pat_id == invoiceDto.PatientId);
            if (!patientExists)
            {
                throw new InvalidOperationException("The specified Patient does not exist");
            }

            invoice.PatientId = invoiceDto.PatientId;
            invoice.DoctorId = invoiceDto.DoctorId;
            invoice.Date = DateTime.ParseExact(invoiceDto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            invoice.PaymentMethod = invoiceDto.PaymentMethod;
            invoice.PaymentDate = DateTime.ParseExact(invoiceDto.PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            invoice.AmountPaid = invoiceDto.AmountPaid;

            // Update Services
            foreach (var service in invoiceDto.Services)
            {
                var existingService = invoice.Services.FirstOrDefault(s => s.Service_id == service.Id);
                if (existingService != null)
                {
                    existingService.Description = service.Description;
                    existingService.Code = service.Code;
                    existingService.Quantity = service.Quantity;
                    existingService.UnitPrice = service.UnitPrice;
                    existingService.Total = service.Total;
                }
                else
                {
                    invoice.Services.Add(new Service
                    {
                        Description = service.Description,
                        Code = service.Code,
                        Quantity = service.Quantity,
                        UnitPrice = service.UnitPrice,
                        Total = service.Total
                    });
                }
            }

            // Update Charges
            foreach (var charge in invoiceDto.Charges)
            {
                var existingCharge = invoice.Charges.FirstOrDefault(c => c.Charge_id == charge.Id);
                if (existingCharge != null)
                {
                    existingCharge.Description = charge.Description;
                    existingCharge.Code = charge.Code;
                    existingCharge.Quantity = charge.Quantity;
                    existingCharge.UnitPrice = charge.UnitPrice;
                    existingCharge.Total = charge.Total;
                }
                else
                {
                    invoice.Charges.Add(new Charge
                    {
                        Description = charge.Description,
                        Code = charge.Code,
                        Quantity = charge.Quantity,
                        UnitPrice = charge.UnitPrice,
                        Total = charge.Total
                    });
                }
            }

            invoice.Subtotal = invoice.Services.Sum(s => s.Total) + invoice.Charges.Sum(c => c.Total);
            decimal taxRate = 4; // 4% tax rate
            invoice.Tax = invoice.Subtotal * (taxRate / 100);
            invoice.Total = invoice.Subtotal + invoice.Tax;

            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            return new InvoiceDTO
            {
                Id = invoice.Invoice_id,
                PatientId = invoice.PatientId,
                DoctorId = invoice.DoctorId,
                PatientName = (await _context.Patients.FirstOrDefaultAsync(p => p.Pat_id == invoice.PatientId))?.Username,
                DoctorUsername = (await _context.Doctors.FirstOrDefaultAsync(d => d.Id == invoice.DoctorId))?.Username,
                Date = invoice.Date.ToString("dd/MM/yyyy"),
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = invoice.Subtotal,
                Tax = invoice.Tax,
                Total = invoice.Total,
                PaymentMethod = invoice.PaymentMethod,
                PaymentDate = invoice.PaymentDate.ToString("dd/MM/yyyy"),
                AmountPaid = invoice.AmountPaid,
                Services = invoice.Services.Select(s => new ServiceDTO
                {
                    Id = s.Service_id,
                    Description = s.Description,
                    Code = s.Code,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    Total = s.Total
                }).ToList(),
                Charges = invoice.Charges.Select(c => new ChargeDTO
                {
                    Id = c.Charge_id,
                    Description = c.Description,
                    Code = c.Code,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    Total = c.Total
                }).ToList()
            };
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Invoice_id == id);

            if (invoice == null)
            {
                return false;
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
