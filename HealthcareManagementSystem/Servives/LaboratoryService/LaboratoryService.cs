using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;
using HealthcareManagementSystem.Models.LabModel;
using System;
using System.Globalization;
using HealthcareManagementSystem.Servives.LaboratoryService;

namespace HealthcareManagementSystem.Services.LaboratoryService
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly ApplicationDbContext _context;

        public LaboratoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GenerateUniqueNumber()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public async Task<LabTestDTO> AddLabTestAsync(CreateLabTestDTO createLabTest)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == createLabTest.LabPatientName);
            if (patient == null)
            {
                throw new KeyNotFoundException("The specified Patient does not exist");
            }

            var labTest = new Lab
            {
                PatientId = patient.Pat_id,
                LabPatientName = patient.Username,
                LabPatAilment = createLabTest.LabPatAilment,
                LabPatNumber = GenerateUniqueNumber(),
                LabPatTests = createLabTest.LabPatTests,
                LabPatResults = createLabTest.LabPatResults,
                LabNumber = GenerateUniqueNumber(),
                LabDateRec = DateTime.ParseExact(createLabTest.LabDateRec, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            };

            _context.LabTests.Add(labTest);
            await _context.SaveChangesAsync();

            return new LabTestDTO
            {
                Id = labTest.Id,
                PatientId = labTest.PatientId,
                PatientName = labTest.LabPatientName,
                LabPatAilment = labTest.LabPatAilment,
                LabPatNumber = labTest.LabPatNumber,
                LabPatTests = labTest.LabPatTests,
                LabPatResults = labTest.LabPatResults,
                LabNumber = labTest.LabNumber,
                LabDateRec = labTest.LabDateRec.ToString("yyyy-MM-dd"),
            };
        }

        public async Task<LabTestDTO> GetAllLabTestByIdAsync(int id)
        {
            var labTest = await _context.LabTests
                .Include(t => t.Patient)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (labTest == null)
            {
                throw new KeyNotFoundException("The specified Lab Test does not exist");
            }

            return new LabTestDTO
            {
                Id = labTest.Id,
                PatientId = labTest.PatientId,
                PatientName = labTest.LabPatientName,
                LabPatAilment = labTest.LabPatAilment,
                LabPatNumber = labTest.LabPatNumber,
                LabPatTests = labTest.LabPatTests,
                LabPatResults = labTest.LabPatResults,
                LabNumber = labTest.LabNumber,
                LabDateRec = labTest.LabDateRec.ToString("yyyy-MM-dd"),
            };
        }

        public async Task<List<LabTestDTO>> GetAllLabTestsAsync()
        {
            return await _context.LabTests
                .Select(d => new LabTestDTO
                {
                    Id = d.Id,
                    PatientId = d.PatientId,
                    PatientName = d.LabPatientName,
                    LabPatAilment = d.LabPatAilment,
                    LabPatNumber = d.LabPatNumber,
                    LabPatTests = d.LabPatTests,
                    LabPatResults = d.LabPatResults,
                    LabNumber = d.LabNumber,
                    LabDateRec = d.LabDateRec.ToString("yyyy-MM-dd"),
                }).ToListAsync();
        }

        public async Task<LabTestDTO> UpdateLabTestAsync(int id, UpdateLabTestDTO updateLabTest)
        {
            var labTest = await _context.LabTests.FindAsync(id);
            if (labTest == null)
            {
                throw new KeyNotFoundException("The specified Lab Test does not exist");
            }

            labTest.LabPatAilment = updateLabTest.LabPatAilment;
            labTest.LabPatTests = updateLabTest.LabPatTests;
            labTest.LabPatResults = updateLabTest.LabPatResults;
            labTest.LabDateRec = DateTime.ParseExact(updateLabTest.LabDateRec, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            _context.LabTests.Update(labTest);
            await _context.SaveChangesAsync();

            return new LabTestDTO
            {
                Id = labTest.Id,
                PatientId = labTest.PatientId,
                PatientName = labTest.LabPatientName,
                LabPatAilment = labTest.LabPatAilment,
                LabPatNumber = labTest.LabPatNumber,
                LabPatTests = labTest.LabPatTests,
                LabPatResults = labTest.LabPatResults,
                LabNumber = labTest.LabNumber,
                LabDateRec = labTest.LabDateRec.ToString("yyyy-MM-dd"),
            };
        }

        public async Task<bool> DeleteLabTestAsync(int id)
        {
            var labTest = await _context.LabTests.FindAsync(id);
            if (labTest == null)
            {
                return false;
            }

            _context.LabTests.Remove(labTest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
