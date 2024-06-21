using HealthcareManagementSystem.Data;

namespace HealthcareManagementSystem.Models.MedicalModel
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }
        public int PatientId { get; set; }

        public string PatientName { get; set; }         
        public Patient Patient { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }
    }
}
