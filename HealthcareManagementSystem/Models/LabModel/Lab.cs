using HealthcareManagementSystem.Data;

namespace HealthcareManagementSystem.Models.LabModel
{
    public class Lab
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string LabPatientName { get; set; }
        public string LabPatAilment { get; set; }
        public string LabPatNumber { get; set; }
        public string LabPatTests { get; set; }
        public string LabPatResults { get; set; }
        public string LabNumber { get; set; }
        public DateTime LabDateRec { get; set; }

        public Patient Patient { get; set; }
    }
}
