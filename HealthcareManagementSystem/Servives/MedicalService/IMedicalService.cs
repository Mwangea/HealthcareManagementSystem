using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicalModel;

namespace HealthcareManagementSystem.Servives.MedicalService
{
    public interface IMedicalService
    {
        Task<MedicalRecordDTO> AddMedicalRecordAsync(CreateMedicalRecordDTO createMedicalRecord);
        Task<List<MedicalRecordDTO>> GetMedicalRecordsByPatientIdAsync(int patientId);

        Task<MedicalRecordDTO> GetMedicalRecordByIdAsync(int id);

        Task<List<MedicalRecordDTO>> GetAllMedicalRecordsAsync();

        Task<MedicalRecordDTO> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordDTO updateMedicalRecord);

        Task<bool> DeleteMedicalRecordAsync(int id);
    }
}
