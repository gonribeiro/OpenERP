using OpenERP.Data;
using OpenERP.Data.Seeders.Auth;
using OpenERP.Data.Seeders.Global;
using OpenERP.Data.Seeders.HumanResource;

namespace OpenERP.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors("_myAllowSpecificOrigins");
            return app;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<AppDbContext>();
                    dbContext.IsSeeding = true; // Prevents auditing during seeding

                    CountrySeeder.Seed(dbContext);
                    StateSeeder.Seed(dbContext);
                    CitySeeder.Seed(dbContext);
                    CompanySeeder.Seed(dbContext);
                    RoleSeeder.Seed(dbContext); // não comente Role
                    UserSeeder.Seed(dbContext); // não comente User
                    RoleUserSeeder.Seed(dbContext);
                    DepartmentSeeder.Seed(dbContext);
                    JobSeeder.Seed(dbContext);
                    //EmployeeFakeSeeder.Seed(dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return app;
        }
    }
}