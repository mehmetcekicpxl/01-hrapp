using PeopleApp.MinimalApi.Models;
using PeopleApp.MinimalApi.Repositories;

namespace PeopleApp.MinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Swager toevoegen aan de applicatie 
            builder.Services.AddEndpointsApiExplorer();
            //swagger gen als het niet al automatisch is toegevoegd dan toevoegen via Swashbuckle.AspNetCore nuget package
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<LocatieRepository>();


            var app = builder.Build();

           

            // Swagger inschakelen
            if (app.Environment.IsDevelopment())
            {
                //swagger  als het niet al automatisch is toegevoegd dan toevoegen via Swashbuckle.AspNetCore nuget package
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //extensie methoden voor endpoints
            app.MapDepartmentEndpoints();
            

            // Onze tijdelijke database (lijst van personen)
            var people = new List<Person>
                {
                    new Person { Id = 1, FirstName = "Alice", LastName = "Johnson" },
                    new Person { Id = 2, FirstName = "Bob", LastName = "Smith" },
                    new Person { Id = 3, FirstName = "Charlie", LastName = "Brown" }
                };

            app.MapGet("/", () => "Hello World!").WithTags("Algemeen");
            app.MapGet("/welcome", () => Results.Ok("Welcome to our API")).WithTags("Algemeen");


            app.MapGet("/person", () =>
            {
                return TypedResults.Ok(new
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Johnson"
                });
            }).WithTags("Person");



            // GET /people -> Haal de volledige lijst op
            app.MapGet("/people", () =>
            {
                return TypedResults.Ok(people);
            }).WithTags("People");



            // GET /people/{id} -> Haal één specifieke persoon op basis van Id
            app.MapGet("/people/{id}", (int id) =>
            {
                // Zoek de persoon in de lijst waarvan het Id overeenkomt
                var person = people.FirstOrDefault(p => p.Id == id);

                // Als de persoon niet bestaat (bijv. id 99), stuur een 404 Not Found
                if (person == null)
                {
                    return Results.NotFound(new { message = "Persoon niet gevonden!" });
                }

                // Als de persoon is gevonden, retourneer deze met een 200 OK
                return TypedResults.Ok(person);
            }).WithTags("People");



            // POST /people -> Voeg een nieuwe persoon toe aan de lijst
            app.MapPost("/people", (Person person) =>
            {
                // Voeg de nieuwe persoon toe aan onze tijdelijke lijst (database)
                people.Add(person);

                // Stuur een 201 Created status terug met de juiste locatie link en het object
                return TypedResults.Created($"/people/{person.Id}", person);
            }).WithTags("People");


            // get /locaties -> haal alle locaties op
            app.MapGet("/locaties", (LocatieRepository locatieRepository) =>
            {
                var locaties = locatieRepository.GetAll();
                return TypedResults.Ok(locaties);
            }).WithTags("Locaties");

            // GET /locaties/{id} -> Haal één specifieke locatie op basis van Id
            app.MapGet("/locaties/{id}", (int id, LocatieRepository locatieRepository) =>
            {
                var locatie = locatieRepository.GetById(id);
                if (locatie == null)
                {
                    return Results.NotFound(new { message = "Locatie niet gevonden!" });
                }
                return TypedResults.Ok(locatie);
            }).WithTags("Locaties");

            // POST /locaties -> Voeg een nieuwe locatie toe aan de lijst
            app.MapPost("/locaties", (Locatie locatie, LocatieRepository locatieRepository) =>
            {
                // Voeg de nieuwe locatie toe aan de database
                locatieRepository.Add(locatie);

                // Stuur een 201 Created status terug.
                // De eerste parameter is de URL waar we deze locatie kunnen vinden.
                // De tweede parameter is het object zelf (inclusief het nieuwe Id).
                return TypedResults.Created($"/locaties/{locatie.Id}", locatie);
            }).WithTags("Locaties");





            //// endpoint grouppen voor locaties
            //// --- YEN? HAL? (MapGroup ile gruplanm??) ---

            //// 1. Maak een groep aan voor alles wat met locaties te maken heeft
            //var locatieGroup = app.MapGroup("/locaties");

            //// 2. Koppel de endpoints aan deze groep (let op: de URL is nu korter!)

            //// GET /locaties (De "/locaties" komt automatisch uit de groep)
            //locatieGroup.MapGet("/", (LocatieRepository locatieRepository) =>
            //{
            //    var locaties = locatieRepository.GetAll();
            //    return TypedResults.Ok(locaties);
            //});

            //// GET /locaties/{id} (De "/locaties/" wordt er automatisch voor geplakt)
            //locatieGroup.MapGet("/{id}", (int id, LocatieRepository locatieRepository) =>
            //{
            //    var locatie = locatieRepository.GetById(id);
            //    if (locatie == null)
            //    {
            //        return Results.NotFound(new { message = "Locatie niet gevonden!" });
            //    }
            //    return TypedResults.Ok(locatie);
            //});

            //// POST /locaties
            //locatieGroup.MapPost("/", (Locatie locatie, LocatieRepository locatieRepository) =>
            //{
            //    locatieRepository.Add(locatie);
            //    return TypedResults.Created($"/locaties/{locatie.Id}", locatie);
            //});

            app.Run();

        }
        // Definieer de structuur (het model) van een persoon
        public class Person
        {
            public int Id { get; set; } = 0;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
        }
    }
}
