using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.MedicineService
{
    public interface IMedicineService
    {
        Task<MedicineDTO> AddMedicineAsync(CreateMedicineDTO createMedicineDTO);
        Task<MedicineDTO> AddStockAsync(int id, int quantity);
        Task<List<MedicineDTO>> GetAllMedicineAsync();
        Task<MedicineDTO> GetMedicineByIdAsync(int id);
        Task<MedicineDTO> UpdateMedicineAsync(int id, UpdateMedicineDTO updateMedicineDTO);
        Task<MedicineDTO> BuyMedicineAsync(int id, int quantity);
        Task DeleteMedicineAsync(int id);
    }
}
