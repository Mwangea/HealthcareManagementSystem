using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HealthcareManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Patient> Patients { get; set; }
      //  public DbSet<Prescription> Prescriptions { get; set; }
        //public DbSet<Invoice> Invoices { get; set; }
        //public DbSet<Report> Reports { get; set; }
        //public DbSet<MedicalRecord> MedicalRecords { get; set; }
        //public DbSet<LabaratoryTest> LabaratoryTests { get; set;  }
        //public DbSet<Surgery> Surgeries { get; set; }
        //public DbSet<Employee> Employees { get; set; }
        //public DbSet<Payroll> Payrolls { get; set; }
        //public DbSet<Vendor> Vendors { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
