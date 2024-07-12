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
                ExpiryDate = medicine.ExpiryDate.ToString("yyyy-MM-dd"),
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
                ExpiryDate = medicine.ExpiryDate.ToString("yyyy-MM-dd"),
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
                    ExpiryDate = d.ExpiryDate.ToString("yyyy-MM-dd"),
                    Manufacturer = d.Manufacturer,

                }).ToListAsync();
        }

        public async Task<MedicineDTO> GetMedicineByIdAsync(int id)
        {
            var medicines = await _context.Medicines.FindAsync(id);

            if (medicines == null)
            {
                throw new KeyNotFoundException("The specified medicine doent not exist");
            }

            return new MedicineDTO
            {
                Id = id,
                ProductName = medicines.ProductName,
                Type = medicines.Type,
                PricePerPack = medicines.PricePerPack,
                Stock = medicines.Stock,
                ExpiryDate = medicines.ExpiryDate.ToString("yyyy-MM-dd"),
                Manufacturer = medicines.Manufacturer,

            };
        }

        public async Task<MedicineDTO> BuyMedicineAsync(int id, int quantity)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                throw new KeyNotFoundException("The specified Medicine does not exist");
            }

            if (medicine.Stock < quantity)
            {
                throw new InvalidOperationException("Not enough stock available");
            }

            medicine.Stock -= quantity;

            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();

            return new MedicineDTO
            {
                Id = medicine.Id,
                ProductName = medicine.ProductName,
                Type = medicine.Type,
                PricePerPack = medicine.PricePerPack,
                Stock = medicine.Stock,
                ExpiryDate = medicine.ExpiryDate.ToString("yyyy-MM-dd"),
                Manufacturer = medicine.Manufacturer
            };
        }

        public async Task<MedicineDTO> UpdateMedicineAsync(int id, UpdateMedicineDTO updateMedicineDTO)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                throw new KeyNotFoundException("The specified Medicine does not exist");
            }

            medicine.ProductName = updateMedicineDTO.ProductName;
            medicine.Type = updateMedicineDTO.Type;
            medicine.PricePerPack = updateMedicineDTO.PricePerPack;
            medicine.Stock = updateMedicineDTO.Stock;
            medicine.ExpiryDate = updateMedicineDTO.ExpiryDate;
            medicine.Manufacturer = updateMedicineDTO.Manufacturer;

            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();

            return new MedicineDTO
            {
                Id = medicine.Id,
                ProductName = medicine.ProductName,
                Type = medicine.Type,
                PricePerPack = medicine.PricePerPack,
                Stock = medicine.Stock,
                ExpiryDate = medicine.ExpiryDate.ToString("yyyy-MM-dd"),
                Manufacturer = medicine.Manufacturer
            };
        }

        public async Task DeleteMedicineAsync(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
                await _context.SaveChangesAsync();
            }
        }

        private MedicineDTO MapToDTO(Medicine medicine)
        {
            return new MedicineDTO
            {
                Id = medicine.Id,
                ProductName = medicine.ProductName,
                Type = medicine.Type,
                PricePerPack = medicine.PricePerPack,
                Stock = medicine.Stock,
                ExpiryDate = medicine.ExpiryDate.ToString("yyyy-MM-dd"), // Format DateTime as needed
                Manufacturer = medicine.Manufacturer
            };
        }

    }
}
