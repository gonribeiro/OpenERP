using OpenERP.Enums.Global;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Seeders.HumanResource
{
    public static class JobSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Jobs.Any())
                return;

            var jobs = new Job[]
            {
                new Job { Name = "Doctor", Currency = Currency.USD },
                new Job { Name = "Engineer", Currency = Currency.USD },
                new Job { Name = "Teacher", Currency = Currency.USD },
                new Job { Name = "Nurse", Currency = Currency.USD },
                new Job { Name = "Accountant", Currency = Currency.USD },
                new Job { Name = "Lawyer", Currency = Currency.USD },
                new Job { Name = "Architect", Currency = Currency.USD },
                new Job { Name = "Pharmacist", Currency = Currency.USD },
                new Job { Name = "Dentist", Currency = Currency.USD },
                new Job { Name = "Software Developer", Currency = Currency.USD },
                new Job { Name = "Data Scientist", Currency = Currency.USD },
                new Job { Name = "Graphic Designer", Currency = Currency.USD },
                new Job { Name = "Marketing Manager", Currency = Currency.USD },
                new Job { Name = "Sales Representative", Currency = Currency.USD },
                new Job { Name = "Financial Analyst", Currency = Currency.USD },
                new Job { Name = "Human Resources Manager", Currency = Currency.USD },
                new Job { Name = "Project Manager", Currency = Currency.USD },
                new Job { Name = "Consultant", Currency = Currency.USD },
                new Job { Name = "Chef", Currency = Currency.USD },
                new Job { Name = "Electrician", Currency = Currency.USD },
                new Job { Name = "Plumber", Currency = Currency.USD },
                new Job { Name = "Mechanic", Currency = Currency.USD },
                new Job { Name = "Pilot", Currency = Currency.USD },
                new Job { Name = "Journalist", Currency = Currency.USD },
                new Job { Name = "Photographer", Currency = Currency.USD },
                new Job { Name = "Actor", Currency = Currency.USD },
                new Job { Name = "Musician", Currency = Currency.USD },
                new Job { Name = "Scientist", Currency = Currency.USD },
                new Job { Name = "Researcher", Currency = Currency.USD },
                new Job { Name = "Veterinarian", Currency = Currency.USD },
                new Job { Name = "Police Officer", Currency = Currency.USD },
                new Job { Name = "Firefighter", Currency = Currency.USD },
                new Job { Name = "Social Worker", Currency = Currency.USD },
                new Job { Name = "Librarian", Currency = Currency.USD },
                new Job { Name = "Translator", Currency = Currency.USD },
                new Job { Name = "Interpreter", Currency = Currency.USD },
                new Job { Name = "Therapist", Currency = Currency.USD },
                new Job { Name = "Psychologist", Currency = Currency.USD },
                new Job { Name = "Economist", Currency = Currency.USD },
                new Job { Name = "Auditor", Currency = Currency.USD },
                new Job { Name = "Banker", Currency = Currency.USD },
                new Job { Name = "Real Estate Agent", Currency = Currency.USD },
                new Job { Name = "Flight Attendant", Currency = Currency.USD },
                new Job { Name = "Professor", Currency = Currency.USD },
                new Job { Name = "Business Analyst", Currency = Currency.USD },
                new Job { Name = "Web Developer", Currency = Currency.USD },
                new Job { Name = "UI/UX Designer", Currency = Currency.USD },
            };

            context.Jobs.AddRange(jobs);
            context.SaveChanges();
        }
    }
}
