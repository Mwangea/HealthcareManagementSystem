namespace HealthcareManagementSystem.Models.MedicineModel
{
    public class Medicine
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Type { get; set; }
        public decimal PricePerPack { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
    }
}
