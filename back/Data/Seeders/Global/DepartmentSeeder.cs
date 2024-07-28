using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Seeders.Global
{
    public static class DepartmentSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Departments.Any())
                return;

            var departments = new Department[]
            {
                new Department { Name = "Human Resources (HR)" },
                new Department { Name = "Finance" },
                new Department { Name = "Accounting" },
                new Department { Name = "Sales" },
                new Department { Name = "Marketing" },
                new Department { Name = "Customer Service" },
                new Department { Name = "Information Technology (IT)" },
                new Department { Name = "Operations" },
                new Department { Name = "Research and Development (R&D)" },
                new Department { Name = "Production" },
                new Department { Name = "Purchasing / Procurement" },
                new Department { Name = "Logistics" },
                new Department { Name = "Quality Assurance (QA)" },
                new Department { Name = "Legal" },
                new Department { Name = "Public Relations (PR)" },
                new Department { Name = "Administration" },
                new Department { Name = "Business Development" },
                new Department { Name = "Compliance" },
                new Department { Name = "Risk Management" },
                new Department { Name = "Corporate Strategy" },
                new Department { Name = "Product Management" },
                new Department { Name = "Supply Chain Management" },
                new Department { Name = "Facilities Management" },
                new Department { Name = "Training and Development" },
                new Department { Name = "Health and Safety" },
            };

            context.Departments.AddRange(departments);
            context.SaveChanges();
        }
    }
}
