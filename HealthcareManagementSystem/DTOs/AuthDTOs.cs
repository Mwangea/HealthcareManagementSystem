using HealthcareManagementSystem.Models.Invoicemodel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace HealthcareManagementSystem.DTOs
{
    // DTOs for Authentication

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }

    public class RegisterResponse
    {
        public string Message { get; set; }
        public string UserId { get; set; }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public string Role { get; set; }
        
    }

    public class AdminLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminLoginResponse
    {
        public string Token { get; set; }
    }

    public class AdminRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }

    public class AdminRegisterResponse
    {
        public string Message { get; set; }
        public string AdminId { get; set; }
    }

    public class DoctorRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Specialty is required")]
        public string Specialty { get; set; }

    }
    public class DoctorRegisterResponse
    {
        public string Message { get; set; }
        public string DoctorId { get; set; }

    }

    public class DoctorLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DoctorLoginResponse
    {
        public string Token { get; set; } 

    }

    public class CreateAppointmentRequest
    {
        [Required]
        public string PatientUsername { get; set; }

        [Required]
        public string DoctorUsername { get; set; }

        [Required]
        public DateTime DateOnly { get; set; }

        [Required]
        public string TimeOnly { get; set; }

        public string Notes { get; set; }

        public DateTime GetAppointmentDateTime()
        {
            return DateOnly.Date + DateTime.ParseExact(TimeOnly, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
        }


    }

    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Date { get; set; }  // Change this to DateTime
        public string Time { get; set; }
    }

    public class InvoiceDTO
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public string PatientName { get; set; }
        public string DoctorUsername { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string InsuranceInformation { get; set; }
        public List<ServiceDTO> Services { get; set; }
        public List<ChargeDTO> Charges { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class UpdateInvoiceDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }

        public List<ServiceDTO> Services { get; set; }
        public List<ChargeDTO> Charges { get; set; }
    }

    public class CreateInvoiceDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string PatientUsername { get; set; }
        public string DoctorUsername { get; set; }
        public DateTime Date { get; set; }
        public List<CreateServiceDTO> Services { get; set; }
        public List<CreateChargeDTO> Charges { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class InvoiceResponse
    {
        public Invoice Invoice { get; set; }
        public string DoctorUsername { get; set; }
        public string PatientUsername { get; set; }
    }


    public class ServiceDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateServiceDTO
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ChargeDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateChargeDTO
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class MedicalRecordDTO
    {
        public int MedicalRecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Date { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }
    }

    public class CreateMedicalRecordDTO
    {
        public string PatientUsername { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }
    }


    public class UpdateMedicalRecordDTO
    {
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }
    }

    public class LabTestDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string LabPatTests { get; set; }
        public string LabPatNumber { get; set; }
        public DateTime LabDateRec { get; set; }
        public string LabPatResults { get; set; }
        public string LabPatAilment { get; set; }
        public string LabNumber { get; set; }
    }

    public class CreateLabTestDTO
    {
        public int PatientId { get; set; }
      //  public string LabPatientName { get; set; }
        public string LabPatAilment { get; set; }
        //public string LabPatNumber { get; set; }
        public string LabPatTests { get; set; }
        public string LabPatResults { get; set; }
       // public string LabNumber { get; set; }
        public DateTime LabDateRec { get; set; }
    }

    public class UpdateLabTestDTO
    {
        public string LabPatTests { get; set; }
        public string LabPatAilment { get; set; }
        public string LabPatResults { get; set; }
        public DateTime LabDateRec { get; set; }
    }

    public class MedicineDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Type { get; set; }
        public decimal PricePerPack { get; set; }
        public int Stock { get; set; }
        public string ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
    }

    public class CreateMedicineDTO
    {
        public string ProductName { get; set; }
        public string Type { get; set; }
        public decimal PricePerPack { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
    }

    public class UpdateMedicineDTO
    {
        public string ProductName { get; set; }
        public string Type { get; set; }
        public decimal PricePerPack { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
    }
}
