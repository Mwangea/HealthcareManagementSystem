using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;

namespace HealthcareManagementSystem.Servives.InvoiceServices
{
    public interface IInvoiceService
    {
        Task<Invoice> AddInvoiceAsync(CreateInvoiceDTO invoiceDto);
        Task<InvoiceDTO> GetInvoiceByIdAsync(int id);
        Task<List<InvoiceDTO>> GetAllInvoicesAsync();
        Task<InvoiceDTO> UpdateInvoiceAsync(int id, UpdateInvoiceDTO invoiceDto);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
