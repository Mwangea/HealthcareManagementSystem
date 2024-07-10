using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthcareManagementSystem.Data;

namespace HealthcareManagementSystem.Servives.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetDoctorIdByUsernameAsync(string username)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username == username);
            return doctor?.Id;
        }

        public async Task<int?> GetPatientIdByUsernameAsync(string username)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username == username);
            return patient?.Pat_id;
        }
    }
    
}
