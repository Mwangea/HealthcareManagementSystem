using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;

namespace HealthcareManagementSystem.Servives.InvoiceServices
{
    public interface IInvoiceService
    {
        Task<Invoice> AddInvoiceAsync(CreateInvoiceDTO invoiceDto);
    }
}
