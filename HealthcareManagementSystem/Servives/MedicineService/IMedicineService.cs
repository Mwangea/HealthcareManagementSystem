using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.MedicineService
{
    public interface IMedicineService
    {
        Task<MedicineDTO> AddMedicineAsync(CreateMedicineDTO createMedicineDTO);
        Task<MedicineDTO> AddStockAsync(int id, int quantity);
        Task<List<MedicineDTO>> GetAllMedicineAsync();
    }
}
