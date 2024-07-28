using OpenERP.Models.Global;

namespace OpenERP.Data.Seeders.Global
{
    public static class CitySeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Cities.Any())
                return;

            // USA
            var newYork = context.States.FirstOrDefault(c => c.Name == "New York");
            var california = context.States.FirstOrDefault(c => c.Name == "California");
            var chicago = context.States.FirstOrDefault(c => c.Name == "Illinois");
            var texas = context.States.FirstOrDefault(c => c.Name == "Texas");
            var phoenix = context.States.FirstOrDefault(c => c.Name == "Arizona");
            var philadelphia = context.States.FirstOrDefault(c => c.Name == "Pennsylvania");
            var jacksonville = context.States.FirstOrDefault(c => c.Name == "Florida");
            var columbus = context.States.FirstOrDefault(c => c.Name == "Ohio");
            var indianapolis = context.States.FirstOrDefault(c => c.Name == "Indiana");
            var charlotte = context.States.FirstOrDefault(c => c.Name == "North Carolina");
            var washington = context.States.FirstOrDefault(c => c.Name == "Washington");
            var denver = context.States.FirstOrDefault(c => c.Name == "Colorado");
            // Brazil
            var saoPaulo = context.States.FirstOrDefault(c => c.Name == "São Paulo");
            var rioDeJaneiro = context.States.FirstOrDefault(c => c.Name == "Rio de Janeiro");
            var salvador = context.States.FirstOrDefault(c => c.Name == "Bahia");
            var fortaleza = context.States.FirstOrDefault(c => c.Name == "Ceará");
            var beloHorizonte = context.States.FirstOrDefault(c => c.Name == "Minas Gerais");
            var manaus = context.States.FirstOrDefault(c => c.Name == "Amazonas");
            var curitiba = context.States.FirstOrDefault(c => c.Name == "Paraná");
            var recife = context.States.FirstOrDefault(c => c.Name == "Pernambuco");
            var portoAlegre = context.States.FirstOrDefault(c => c.Name == "Rio Grande do Sul");
            var belém = context.States.FirstOrDefault(c => c.Name == "Pará");
            var goiânia = context.States.FirstOrDefault(c => c.Name == "Goiás");
            var saoLuis = context.States.FirstOrDefault(c => c.Name == "Maranhão");
            var maceio = context.States.FirstOrDefault(c => c.Name == "Alagoas");
            var teresina = context.States.FirstOrDefault(c => c.Name == "Piauí");
            var campoGrande = context.States.FirstOrDefault(c => c.Name == "Mato Grosso do Sul");
            var natal = context.States.FirstOrDefault(c => c.Name == "Rio Grande do Norte");

            var cities = new City[]
            {
                // USA
                new City { Name = "New York City", StateId = newYork.Id },
                new City { Name = "Los Angeles", StateId = california.Id },
                new City { Name = "Chicago", StateId = chicago.Id },
                new City { Name = "Houston", StateId = texas.Id },
                new City { Name = "Phoenix", StateId = phoenix.Id },
                new City { Name = "Philadelphia", StateId = philadelphia.Id },
                new City { Name = "San Antonio", StateId = texas.Id },
                new City { Name = "San Diego", StateId = california.Id },
                new City { Name = "Dallas", StateId = texas.Id },
                new City { Name = "San Jose", StateId = california.Id },
                new City { Name = "Austin", StateId = texas.Id },
                new City { Name = "Jacksonville", StateId = jacksonville.Id },
                new City { Name = "San Francisco", StateId = california.Id },
                new City { Name = "Columbus", StateId = columbus.Id },
                new City { Name = "Indianapolis", StateId = indianapolis.Id },
                new City { Name = "Fort Worth", StateId = texas.Id },
                new City { Name = "Charlotte", StateId = charlotte.Id },
                new City { Name = "Seattle", StateId = washington.Id },
                new City { Name = "Denver", StateId = denver.Id },
                // Brazil
                new City { Name = "São Paulo", StateId = saoPaulo.Id },
                new City { Name = "Rio de Janeiro", StateId = rioDeJaneiro.Id },
                new City { Name = "Salvador", StateId = salvador.Id },
                new City { Name = "Fortaleza", StateId = fortaleza.Id },
                new City { Name = "Belo Horizonte", StateId = beloHorizonte.Id },
                new City { Name = "Manaus", StateId = manaus.Id },
                new City { Name = "Curitiba", StateId = curitiba.Id },
                new City { Name = "Recife", StateId = recife.Id },
                new City { Name = "Porto Alegre", StateId = portoAlegre.Id },
                new City { Name = "Belém", StateId = belém.Id },
                new City { Name = "Goiânia", StateId = goiânia.Id },
                new City { Name = "Guarulhos", StateId = saoPaulo.Id },
                new City { Name = "Campinas", StateId = saoPaulo.Id },
                new City { Name = "São Luís", StateId = saoLuis.Id },
                new City { Name = "São Gonçalo", StateId = rioDeJaneiro.Id },
                new City { Name = "Maceió", StateId = maceio.Id },
                new City { Name = "Teresina", StateId = teresina.Id },
                new City { Name = "Campo Grande", StateId = campoGrande.Id },
                new City { Name = "Natal", StateId = natal.Id }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
