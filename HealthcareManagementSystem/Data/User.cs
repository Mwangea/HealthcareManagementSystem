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

        [Required]
        [MaxLength(200)]
        public string Pat_fname { get; set; }

        [Required]
        [MaxLength(200)]
        public string Pat_lname { get; set; }

        [MaxLength(200)]
        public string Pat_dob { get; set; }

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
    }

   

}

