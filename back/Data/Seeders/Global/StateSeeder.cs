using OpenERP.Models.Global;

namespace OpenERP.Data.Seeders.Global
{
    public static class StateSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.States.Any())
                return;

            var usa = context.Countries.FirstOrDefault(c => c.Name == "United States");
            var brazil = context.Countries.FirstOrDefault(c => c.Name == "Brazil");

            var states = new State[]
            {
                // USA
                new State { Name = "Alabama", CountryId = usa.Id },
                new State { Name = "Alaska", CountryId = usa.Id },
                new State { Name = "Arizona", CountryId = usa.Id },
                new State { Name = "Arkansas", CountryId = usa.Id },
                new State { Name = "California", CountryId = usa.Id },
                new State { Name = "Colorado", CountryId = usa.Id },
                new State { Name = "Connecticut", CountryId = usa.Id },
                new State { Name = "Delaware", CountryId = usa.Id },
                new State { Name = "Florida", CountryId = usa.Id },
                new State { Name = "Georgia", CountryId = usa.Id },
                new State { Name = "Hawaii", CountryId = usa.Id },
                new State { Name = "Idaho", CountryId = usa.Id },
                new State { Name = "Illinois", CountryId = usa.Id },
                new State { Name = "Indiana", CountryId = usa.Id },
                new State { Name = "Iowa", CountryId = usa.Id },
                new State { Name = "Kansas", CountryId = usa.Id },
                new State { Name = "Kentucky", CountryId = usa.Id },
                new State { Name = "Louisiana", CountryId = usa.Id },
                new State { Name = "Maine", CountryId = usa.Id },
                new State { Name = "Maryland", CountryId = usa.Id },
                new State { Name = "Massachusetts", CountryId = usa.Id },
                new State { Name = "Michigan", CountryId = usa.Id },
                new State { Name = "Minnesota", CountryId = usa.Id },
                new State { Name = "Mississippi", CountryId = usa.Id },
                new State { Name = "Missouri", CountryId = usa.Id },
                new State { Name = "Montana", CountryId = usa.Id },
                new State { Name = "Nebraska", CountryId = usa.Id },
                new State { Name = "Nevada", CountryId = usa.Id },
                new State { Name = "New Hampshire", CountryId = usa.Id },
                new State { Name = "New Jersey", CountryId = usa.Id },
                new State { Name = "New Mexico", CountryId = usa.Id },
                new State { Name = "New York", CountryId = usa.Id },
                new State { Name = "North Carolina", CountryId = usa.Id },
                new State { Name = "North Dakota", CountryId = usa.Id },
                new State { Name = "Ohio", CountryId = usa.Id },
                new State { Name = "Oklahoma", CountryId = usa.Id },
                new State { Name = "Oregon", CountryId = usa.Id },
                new State { Name = "Pennsylvania", CountryId = usa.Id },
                new State { Name = "Rhode Island", CountryId = usa.Id },
                new State { Name = "South Carolina", CountryId = usa.Id },
                new State { Name = "South Dakota", CountryId = usa.Id },
                new State { Name = "Tennessee", CountryId = usa.Id },
                new State { Name = "Texas", CountryId = usa.Id },
                new State { Name = "Utah", CountryId = usa.Id },
                new State { Name = "Vermont", CountryId = usa.Id },
                new State { Name = "Virginia", CountryId = usa.Id },
                new State { Name = "Washington", CountryId = usa.Id },
                new State { Name = "West Virginia", CountryId = usa.Id },
                new State { Name = "Wisconsin", CountryId = usa.Id },
                new State { Name = "Wyoming", CountryId = usa.Id },
                // Brazil
                new State { Name = "Acre", CountryId = brazil.Id },
                new State { Name = "Alagoas", CountryId = brazil.Id },
                new State { Name = "Amapá", CountryId = brazil.Id },
                new State { Name = "Amazonas", CountryId = brazil.Id },
                new State { Name = "Bahia", CountryId = brazil.Id },
                new State { Name = "Ceará", CountryId = brazil.Id },
                new State { Name = "Espírito Santo", CountryId = brazil.Id },
                new State { Name = "Goiás", CountryId = brazil.Id },
                new State { Name = "Maranhão", CountryId = brazil.Id },
                new State { Name = "Mato Grosso", CountryId = brazil.Id },
                new State { Name = "Mato Grosso do Sul", CountryId = brazil.Id },
                new State { Name = "Minas Gerais", CountryId = brazil.Id },
                new State { Name = "Pará", CountryId = brazil.Id },
                new State { Name = "Paraíba", CountryId = brazil.Id },
                new State { Name = "Paraná", CountryId = brazil.Id },
                new State { Name = "Pernambuco", CountryId = brazil.Id },
                new State { Name = "Piauí", CountryId = brazil.Id },
                new State { Name = "Rio de Janeiro", CountryId = brazil.Id },
                new State { Name = "Rio Grande do Norte", CountryId = brazil.Id },
                new State { Name = "Rio Grande do Sul", CountryId = brazil.Id },
                new State { Name = "Rondônia", CountryId = brazil.Id },
                new State { Name = "Roraima", CountryId = brazil.Id },
                new State { Name = "Santa Catarina", CountryId = brazil.Id },
                new State { Name = "São Paulo", CountryId = brazil.Id },
                new State { Name = "Sergipe", CountryId = brazil.Id },
                new State { Name = "Tocantins", CountryId = brazil.Id },
            };

            context.States.AddRange(states);
            context.SaveChanges();
        }
    }
}
