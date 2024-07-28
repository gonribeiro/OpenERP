using OpenERP.Models.Auth;

namespace OpenERP.Data.Seeders.Auth
{
    public static class RoleSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Roles.Any())
                return;

            var role = new Role[]
            {
                new Role { Name = "Admin" },
            };

            context.Roles.AddRange(role);
            context.SaveChanges();
        }
    }
}