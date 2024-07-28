using Bogus;
using OpenERP.Enums.Global;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Seeders.HumanResource
{
    public static class EmployeeFakeSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Employees.Any())
                return;

            var departments = context.Departments.ToList();
            var random = new Random();

            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Birthdate, f => f.Date.Past(40, DateTime.Today.AddYears(-18)))
                .RuleFor(e => e.MaritalStatus, f => f.PickRandom<MaritalStatus>())
                .RuleFor(e => e.NationalityId, f => f.Random.Int(1, 10))
                .RuleFor(e => e.Address, f => f.Address.StreetAddress())
                .RuleFor(e => e.ZipCode, f => f.Address.ZipCode())
                .RuleFor(e => e.CityId, f => f.Random.Int(1, 20))
                .RuleFor(e => e.SocialSecurityNumber, f => f.Random.Replace("###-##-####"))
                .RuleFor(e => e.InactiveDate, f => null)
                .RuleFor(e => e.User, f => null);

            var employees = employeeFaker.Generate(50);

            context.Employees.AddRange(employees);
            context.SaveChanges();

            var departmentEmployeeFakers = new List<DepartmentEmployee>();

            foreach (var employee in employees)
            {
                var departmentEmployeeFaker = new DepartmentEmployee
                {
                    DepartmentId = random.Next(1, 10),
                    EmployeeId = random.Next(1, 50)
                };

                departmentEmployeeFakers.Add(departmentEmployeeFaker);
            }

            context.DepartmentEmployee.AddRange(departmentEmployeeFakers);
            context.SaveChanges();

            var remunerationFaker = new Faker<Remuneration>()
                .RuleFor(r => r.EmployeeId, f => f.PickRandom(employees).Id)
                .RuleFor(r => r.Currency, f => f.PickRandom<Currency>())
                .RuleFor(r => r.BaseSalary, f => f.Finance.Amount(1000, 5000))
                .RuleFor(r => r.Bonus, f => f.Finance.Amount(500, 1000))
                .RuleFor(r => r.Commission, f => f.Finance.Amount(300, 500))
                .RuleFor(r => r.OtherAllowances, f => f.Finance.Amount(100, 300))
                .RuleFor(r => r.OtherAllowancesDescription, f => f.Lorem.Sentence())
                .RuleFor(r => r.StartDate, f => f.Date.Past(1, DateTime.Today))
                .RuleFor(r => r.EndDate, f => null);

            var remunerations = remunerationFaker.Generate(employees.Count);

            context.Remunerations.AddRange(remunerations);
            context.SaveChanges();

            var vacationFaker = new Faker<Vacation>()
                .RuleFor(v => v.EmployeeId, f => f.PickRandom(employees).Id)
                .RuleFor(v => v.StartDate, f => f.Date.Past(30, DateTime.Today))
                .RuleFor(v => v.EndDate, f => f.Date.Future(30, DateTime.Today))
                .RuleFor(v => v.Employee, f => null);

            var vacations = vacationFaker.Generate(20);

            context.Vacations.AddRange(vacations);
            context.SaveChanges();
        }
    }
}