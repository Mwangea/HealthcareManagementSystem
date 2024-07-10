using HealthcareManagementSystem.Models.MedicalModel;
using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }

    public class Patient
    {
        [Key]
        public int Pat_id { get; set; }

        [MaxLength(200)]
        public string? Pat_dob { get; set; }

        [MaxLength(200)]
        public string Pat_age { get; set; }

        [MaxLength(200)]
        public string Pat_number { get; set; }

        [MaxLength(200)]
        public string Pat_addr { get; set; }

        [MaxLength(200)]
        public string Pat_phone { get; set; }

        [MaxLength(200)]
        public string Pat_type { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Pat_date_joined { get; set; } = DateTime.Now;

        [MaxLength(200)]
        public string Pat_ailment { get; set; }

        [MaxLength(200)]
        public string Pat_discharge_status { get; set; }
        public string Pat_blood_group { get; set; }
        public string Gender { get; set; }
        [MaxLength(100)]  // Adjust max length as needed
        public string Username { get; set; }

        // public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }

   

}

