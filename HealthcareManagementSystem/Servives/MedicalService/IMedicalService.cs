using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicalModel;

namespace HealthcareManagementSystem.Servives.MedicalService
{
    public interface IMedicalService
    {
        Task<MedicalRecordDTO> AddMedicalRecordAsync(CreateMedicalRecordDTO createMedicalRecord);
        Task<List<MedicalRecordDTO>> GetMedicalRecordsByPatientIdAsync(int patientId);
    }
}
