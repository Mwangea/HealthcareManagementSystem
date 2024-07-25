using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.Invoicemodel;

namespace HealthcareManagementSystem.Servives.InvoiceServices
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> AddInvoiceAsync(CreateInvoiceDTO invoiceDto);
        Task<InvoiceDTO> GetInvoiceByIdAsync(int id);
        Task<List<InvoiceDTO>> GetAllInvoicesAsync();
        Task<List<InvoiceDTO>> GetInvoicesByDoctorIdAsync(int doctorId);
        Task<InvoiceDTO> UpdateInvoiceAsync(int id, UpdateInvoiceDTO invoiceDto);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
