using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.DTOs;
using HealthcareManagementSystem.Models.MedicineModel;
using Microsoft.EntityFrameworkCore;

namespace HealthcareManagementSystem.Servives.MedicineService
{
    public class MedicineService : IMedicineService
    {
        private readonly ApplicationDbContext _context;

        public MedicineService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicineDTO> AddMedicineAsync(CreateMedicineDTO createMedicineDTO)
        {
            var medicine = new Medicine
            {
                ProductName = createMedicineDTO.ProductName,
                Type = createMedicineDTO.Type,
                PricePerPack = createMedicineDTO.PricePerPack,
                Stock = createMedicineDTO.Stock,
                ExpiryDate = createMedicineDTO.ExpiryDate,
                Manufacturer = createMedicineDTO.Manufacturer,
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return new MedicineDTO
            {
                Id = medicine.Id,
                ProductName = medicine.ProductName,
                Type = medicine.Type,
                PricePerPack = medicine.PricePerPack,
                Stock = medicine.Stock,
                ExpiryDate = medicine.ExpiryDate,
                Manufacturer = medicine.Manufacturer
            };
        }

        public async Task<MedicineDTO> AddStockAsync(int id, int quantity)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                throw new KeyNotFoundException("The specified Medicine does not exist");
            }

            medicine.Stock += quantity;

            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();

            return new MedicineDTO
            {
                Id = medicine.Id,
                ProductName = medicine.ProductName,
                Type = medicine.Type,
                PricePerPack = medicine.PricePerPack,
                Stock = medicine.Stock,
                ExpiryDate = medicine.ExpiryDate,
                Manufacturer = medicine.Manufacturer
            };
        }

        public async Task<List<MedicineDTO>> GetAllMedicineAsync()
        {
            return await _context.Medicines
                .Select(d => new MedicineDTO
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    Type = d.Type,
                    PricePerPack = d.PricePerPack,
                    Stock = d.Stock,
                    ExpiryDate = d.ExpiryDate,
                    Manufacturer = d.Manufacturer,

                }).ToListAsync();
        }

    }
}
