using HealthcareManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Servives
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        
        public PatientService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Select(p => new Patient
                {
                    Pat_id = p.Pat_id,
                    Username = p.Username ?? string.Empty,
                    //Pat_fname = p.Pat_fname ?? string.Empty,
                    // Pat_lname = p.Pat_lname ?? string.Empty,
                    Pat_dob = p.Pat_dob ?? string.Empty,
                    Pat_age = p.Pat_age ?? string.Empty,
                    Pat_number = p.Pat_number ?? string.Empty,
                    Pat_addr = p.Pat_addr ?? string.Empty,
                    Pat_phone = p.Pat_phone ?? string.Empty,
                    Pat_type = p.Pat_type ?? string.Empty,
                    Pat_date_joined = p.Pat_date_joined,
                    Pat_ailment = p.Pat_ailment ?? string.Empty,
                    Pat_discharge_status = p.Pat_discharge_status ?? string.Empty,
                    Pat_blood_group = p.Pat_blood_group ?? string.Empty,
                    Gender = p.Gender ?? string.Empty
                }).ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(int id, Patient updatedPatient)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                patient.Username = updatedPatient.Username;
                patient.Pat_dob = updatedPatient.Pat_dob;
                patient.Pat_age = updatedPatient.Pat_age;
                patient.Pat_number = updatedPatient.Pat_number;
                patient.Pat_addr = updatedPatient.Pat_addr;
                patient.Pat_phone = updatedPatient.Pat_phone;
                patient.Pat_type = updatedPatient.Pat_type;
                patient.Pat_date_joined = updatedPatient.Pat_date_joined;
                patient.Pat_ailment = updatedPatient.Pat_ailment;
                patient.Pat_discharge_status = updatedPatient.Pat_discharge_status;
                patient.Pat_blood_group = updatedPatient.Pat_blood_group;
                patient.Gender = updatedPatient.Gender;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeletePatientAsync (int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
