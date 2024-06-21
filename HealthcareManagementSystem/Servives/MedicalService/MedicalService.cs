using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicalModel;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Servives.MedicalService
{
    public class MedicalService : IMedicalService
    {
        private readonly ApplicationDbContext _context;

        public  MedicalService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecordDTO> AddMedicalRecordAsync(CreateMedicalRecordDTO createMedicalRecord)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Pat_id == createMedicalRecord.PatientId);
            if (patient == null)
            {
                throw new InvalidOperationException("The specified Patient does not exist");
            }

            var medicalRecord = new MedicalRecord
            {
                PatientId = createMedicalRecord.PatientId,
                Date = createMedicalRecord.Date,
                Diagnosis = createMedicalRecord.Diagnosis,
                Treatment = createMedicalRecord.Treatment,
                Notes = createMedicalRecord.Notes,
            };
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();

            return new MedicalRecordDTO
            {
                MedicalRecordId = medicalRecord.MedicalRecordId,
                PatientId = medicalRecord.PatientId,
                PatientName = $"{patient.Pat_fname} {patient.Pat_lname}",
                Date = medicalRecord.Date,
                Diagnosis = medicalRecord.Diagnosis,
                Treatment = medicalRecord.Treatment,
                Notes = medicalRecord.Notes,

            };
        }

        public async Task<List<MedicalRecordDTO>> GetMedicalRecordsByPatientIdAsync(int id)
        {
            var medicalRecords = await _context.MedicalRecords
                .Where(m => m.PatientId == id)
                .Include(m => m.Patient)
                .ToListAsync();

            return medicalRecords.Select(m => new MedicalRecordDTO 
            {
              MedicalRecordId = m.MedicalRecordId,
              PatientId = m.PatientId,
                PatientName = $"{m.Patient.Pat_fname} {m.Patient.Pat_lname}",
                Date = m.Date,
              Diagnosis = m.Diagnosis,
              Treatment = m.Treatment, 
              Notes = m.Notes,
            }).ToList();

            //return medicalRecords;
        }
    }
}
