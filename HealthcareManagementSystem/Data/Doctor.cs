namespace HealthcareManagementSystem.Data
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }

    }

    public class DoctorDT0s
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }
    }
    public class DoctorUpdateRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }
        // Add other properties as needed
    }
}
