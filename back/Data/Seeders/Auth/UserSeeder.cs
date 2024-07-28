using OpenERP.Models.Auth;
using OpenERP.Services.Auth;

namespace OpenERP.Data.Seeders.Auth
{
    public static class UserSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any())
                return;

            var password = PasswordService.HashPassword("Admin");

            var user = new User
            {
                Username = "Admin",
                Password = password,
                CreatedAt = DateTime.UtcNow,
            };

            context.Users.AddAsync(user);
            context.SaveChanges();
        }
    }
}
