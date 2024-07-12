using HealthcareManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HealthcareManagementSystem.Models.Invoicemodel
{
    
    public class Invoice
    {
            [Key]
            public int Invoice_id { get; set; }

            public int PatientId { get; set; }
            public int DoctorId { get; set; }
           // public string PatientUsername { get; set; }
           // public string DoctorUsername { get; set; }
            public DateTime Date { get; set; }
            public string InvoiceNumber { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Tax { get; set; }
            public decimal Total { get; set; }
            public string PaymentMethod { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal AmountPaid { get; set; }

            public List<Service> Services { get; set; }
            public List<Charge> Charges { get; set; }
        }

         public class Service
        {
        [Key]
        public int Service_id { get; set; }

        public int Invoice_id { get; set; }

        [ForeignKey("Invoice_id")]
        [JsonIgnore]
        public Invoice Invoice { get; set; }

        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }

    public class Charge
    {
        [Key]
        public int Charge_id { get; set; }

        public int Invoice_id { get; set; }

        [ForeignKey("Invoice_id")]
        [JsonIgnore]
        public Invoice Invoice { get; set; }

        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }


}
