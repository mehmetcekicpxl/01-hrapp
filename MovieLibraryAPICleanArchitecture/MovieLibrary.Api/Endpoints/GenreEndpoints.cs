using MovieLibrary.Api.Dto;
using MovieLibrary.Application.Repositories;
using MovieLibrary.Domain.Entities;

namespace MovieLibrary.Api.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres").WithTags("Genres");

        group.MapGet("/", GetGenres);
        group.MapPost("/", CreateGenre);
        group.MapGet("/{id}", GetGenreById);
        group.MapPut("/{id}", UpdateGenre);
        group.MapDelete("/{id}", DeleteGenre);
    }

    private static IResult GetGenres(IGenreRepository genreRepository)
    {
        var genres = genreRepository.GetAll();
        return Results.Ok(genres);
    }

    private static IResult CreateGenre(CreateGenreDto genre, IGenreRepository genreRepository)
    {
        var newGenre = new Genre
        {
            Name = genre.Name
        };

        genreRepository.Add(newGenre);
        genreRepository.SaveChanges();

        return Results.Created($"/genres/{newGenre.Id}", newGenre);
    }

    // GET /genres/{id}
    private static IResult GetGenreById(int id, IGenreRepository genreRepository)
    {
        // Zoek het genre in de database
        var genre = genreRepository.GetById(id);

        // Retourneer 404 Not Found als het niet bestaat
        if (genre == null)
        {
            return Results.NotFound(new { message = "Genre niet gevonden." });
        }

        // Retourneer 200 OK met de data
        return Results.Ok(genre);
    }

    // PUT /genres/{id}
    private static IResult UpdateGenre(int id, UpdateGenreDto dto, IGenreRepository genreRepository)
    {
        // Controleer eerst of het genre wel bestaat
        var genre = genreRepository.GetById(id);
        if (genre == null)
        {
            return Results.NotFound(new { message = "Genre niet gevonden." });
        }

        // Update enkel de toegestane velden via de DTO
        genre.Name = dto.Name;

        // Sla de wijzigingen op in de database
        genreRepository.Update(genre);

        // Retourneer 204 No Content (succesvol, maar we sturen geen data terug)
        return Results.NoContent();
    }

    // DELETE /genres/{id}
    private static IResult DeleteGenre(int id, IGenreRepository genreRepository)
    {
        // Controleer of het genre bestaat voordat we het verwijderen
        var genre = genreRepository.GetById(id);
        if (genre == null)
        {
            return Results.NotFound(new { message = "Genre niet gevonden." });
        }

        // Verwijder het uit de database
        genreRepository.Delete(genre);
        genreRepository.SaveChanges();

        // Retourneer 204 No Content
        return Results.NoContent();
    }
}