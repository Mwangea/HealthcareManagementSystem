using HealthcareManagementSystem.DTOs;

namespace HealthcareManagementSystem.Servives.LaboratoryService
{
    public interface ILaboratoryService
    {
        Task<LabTestDTO> AddLabTestAsync(CreateLabTestDTO createLabTest);
        Task<LabTestDTO> GetAllLabTestByIdAsync(int id);

        Task<List<LabTestDTO>> GetAllLabTestsAsync();

        Task<LabTestDTO> UpdateLabTestAsync(int id, UpdateLabTestDTO updateLabTestDTO);

        Task<bool> DeleteLabTestAsync(int id);
    }
}
