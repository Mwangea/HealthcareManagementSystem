using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicalModel;
using HealthcareManagementSystem.Servives.MedicalService;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HealthcareManagementSystem.Services.MedicalService
{
    public class MedicalService : IMedicalService
    {
        private readonly ApplicationDbContext _context;

        public MedicalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecordDTO> AddMedicalRecordAsync(CreateMedicalRecordDTO createMedicalRecord)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == createMedicalRecord.PatientUsername);
            if (patient == null)
            {
                throw new InvalidOperationException("The specified Patient does not exist");
            }

            var medicalRecord = new MedicalRecord
            {
                PatientId = patient.Pat_id,
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
                PatientName = medicalRecord.Patient.Username,
                Date = medicalRecord.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Diagnosis = medicalRecord.Diagnosis,
                Treatment = medicalRecord.Treatment,
                Notes = medicalRecord.Notes,
            };
        }

        public async Task<MedicalRecordDTO> GetMedicalRecordByIdAsync(int id)
        {
            var medicalRecords = await _context.MedicalRecords
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.MedicalRecordId == id);

            if (medicalRecords == null)
            {
                throw new KeyNotFoundException("The specified Medical Record does not exist");
            }

            return new MedicalRecordDTO
            {
                MedicalRecordId = medicalRecords.MedicalRecordId,
                PatientId = medicalRecords.PatientId,
                PatientName = medicalRecords.Patient.Username,
                Date = medicalRecords.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Diagnosis = medicalRecords.Diagnosis,
                Treatment = medicalRecords.Treatment,
                Notes = medicalRecords.Notes
            };
        }

        public async Task<List<MedicalRecordDTO>> GetAllMedicalRecordsAsync()
        {
            var medicalRecords = await _context.MedicalRecords
                .Include(m => m.Patient)
                .ToListAsync();

            return medicalRecords.Select(m => new MedicalRecordDTO
            {
                MedicalRecordId = m.MedicalRecordId,
                PatientId = m.PatientId,
                PatientName = m.Patient.Username,
                Date = m.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Diagnosis = m.Diagnosis,
                Treatment = m.Treatment,
                Notes = m.Notes,
            }).ToList();
        }

        public async Task<List<MedicalRecordDTO>> GetMedicalRecordsByPatientIdAsync(int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Pat_id == id);
            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found");
            }

            var medicalRecords = await _context.MedicalRecords
                .Where(m => m.PatientId == patient.Pat_id)
                .Include(m => m.Patient)
                .ToListAsync();

            return medicalRecords.Select(m => new MedicalRecordDTO
            {
                MedicalRecordId = m.MedicalRecordId,
                PatientId = m.PatientId,
                PatientName = m.Patient.Username,
                Date = m.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Diagnosis = m.Diagnosis,
                Treatment = m.Treatment,
                Notes = m.Notes,
            }).ToList();
        }

        public async Task<MedicalRecordDTO> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordDTO updateMedicalRecord)
        {
            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.MedicalRecordId == id);

            if (medicalRecord == null)
            {
                throw new InvalidOperationException("The specified Medical Record does not exist");
            }

            medicalRecord.Date = updateMedicalRecord.Date;
            medicalRecord.Diagnosis = updateMedicalRecord.Diagnosis;
            medicalRecord.Treatment = updateMedicalRecord.Treatment;
            medicalRecord.Notes = updateMedicalRecord.Notes;

            _context.MedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();

            return new MedicalRecordDTO
            {
                MedicalRecordId = medicalRecord.MedicalRecordId,
                PatientId = medicalRecord.PatientId,
                PatientName = medicalRecord.Patient.Username,
                Date = medicalRecord.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Diagnosis = medicalRecord.Diagnosis,
                Treatment = medicalRecord.Treatment,
                Notes = medicalRecord.Notes,
            };
        }

        public async Task<bool> DeleteMedicalRecordAsync(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return false;
            }

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
