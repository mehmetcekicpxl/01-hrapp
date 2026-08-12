using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PeopleApp.MinimalApi.Models;
using PeopleApp.MinimalApi.Repositories;

namespace PeopleApp.MinimalApi.Endpoints
{
    // De klasse moet statisch zijn voor extension methods
    public static class LocatieEndpoints
    {
        // Het woordje 'this' is hier essentieel
        public static void MapLocatieEndpoints(this IEndpointRouteBuilder app)
        {
            // 1. Maak de groep aan
            var group = app.MapGroup("/locaties").WithTags("Locatie");

            // 2. Koppel de endpoints. 
            // Let op: LocatieRepository werkt hier precies hetzelfde (Dependency Injection)!
            group.MapGet("/", (LocatieRepository locatieRepository) =>
            {
                var locaties = locatieRepository.GetAll();
                return TypedResults.Ok(locaties);
            });

            group.MapGet("/{id}", (int id, LocatieRepository locatieRepository) =>
            {
                var locatie = locatieRepository.GetById(id);
                if (locatie == null)
                {
                    return Results.NotFound(new { message = "Locatie niet gevonden!" });
                }
                return TypedResults.Ok(locatie);
            });

            group.MapPost("/", (Locatie locatie, LocatieRepository locatieRepository) =>
            {
                locatieRepository.Add(locatie);
                return TypedResults.Created($"/locaties/{locatie.Id}", locatie);
            });
        }
    }
}