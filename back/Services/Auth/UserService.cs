using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Auth;

namespace OpenERP.Services.Auth
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Where(c => c.InactiveDate == null)
                .Where(c => c.Username == username)
                .Include(c => c.RoleUser)
                    .ThenInclude(d => d.Role)
                .Include(c => c.Employee)
                    .Where(c => c.Employee.InactiveDate == null)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Where(c => c.InactiveDate == null)
                .Where(c => c.Id == id)
                .Include(c => c.RoleUser)
                    .ThenInclude(d => d.Role)
                .Include(c => c.Employee)
                    .Where(c => c.Employee.InactiveDate == null)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
