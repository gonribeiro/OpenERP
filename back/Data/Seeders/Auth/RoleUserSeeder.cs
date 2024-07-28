using OpenERP.Models.Auth;

namespace OpenERP.Data.Seeders.Auth
{
    public static class RoleUserSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.RoleUser.Any())
                return;

            var roleUser = new RoleUser[]
            {
                new RoleUser { UserId = 1, RoleId = 1 }, // Admin
            };

            context.RoleUser.AddRange(roleUser);
            context.SaveChanges();
        }
    }
}