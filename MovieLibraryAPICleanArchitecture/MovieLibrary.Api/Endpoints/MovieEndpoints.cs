using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Api.Dto;
using MovieLibrary.Application.Services;
using MovieLibrary.Domain.Entities;

namespace MovieLibrary.Api.Endpoints;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/movies").WithTags("Movies");

        group.MapGet("/", GetMovies);
        group.MapGet("/{id}", GetMovieById);
        group.MapPost("/", CreateMovie);
        group.MapDelete("/{id}", DeleteMovie);
        group.MapPatch("/{id}", PatchMovie);
        group.MapPut("/{id}", UpdateMovie);
    }

    private static IResult GetMovies(IMovieService movieService)
    {
        var movies = movieService.GetAllMovies();
        return Results.Ok(movies);
    }

    private static IResult GetMovieById(int id, IMovieService movieService)
    {
        var movie = movieService.GetMovieById(id);

        if (movie == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(movie);
    }

    private static IResult CreateMovie(CreateMovieDto dto, IMovieService movieService)
    {
        // 1. We kopiëren de eenvoudige gegevens die we van de klant (DTO) ontvangen, 
        // naar de grote en complexe Movie-klasse (Entity) die de database begrijpt.
        var newMovie = new Movie
        {
            Title = dto.Title,
            ReleaseYear = dto.ReleaseYear,
            GenreId = dto.GenreId
            // Let op: EF Core zal de Id toewijzen. EF Core zal ook het Genre-object uit de database ophalen en zelf koppelen.
        };

        // 2. We sturen de door ons gemaakte klasse naar de service
        movieService.AddMovie(newMovie);

        // 3. We geven aan dat het succesvol is aangemaakt
        return Results.Created($"/movies/{newMovie.Id}", newMovie);
    }

    private static IResult DeleteMovie(int id, IMovieService movieService)
    {
        var deleted = movieService.DeleteMovie(id);

        if (!deleted)
        {
            return Results.NotFound();
        }

        return Results.NoContent();
    }

    // PATCH /movies/{id}
    private static IResult PatchMovie(int id, PatchMovieDto dto, IMovieService movieService)
    {
        // Controleer of de film bestaat
        var movie = movieService.GetMovieById(id);
        if (movie == null)
        {
            return Results.NotFound(new { message = "Film niet gevonden." });
        }

        // Werk de velden bij met de data uit de DTO
        if (dto.Title != null)
        {
            movie.Title = dto.Title;
        }
        if (dto.ReleaseYear.HasValue)
        {
            movie.ReleaseYear = dto.ReleaseYear.Value;
        }
        if (dto.GenreId.HasValue)
        {
            movie.GenreId = dto.GenreId.Value;
        }

        // Sla op in de database
        movieService.UpdateMovie(movie);

        // Retourneer 204 No Content
        return Results.NoContent();
    }

    // PUT /movies/{id} -> Overschrijft alles
    private static IResult UpdateMovie(int id, UpdateMovieDto dto, IMovieService movieService)
    {
        var movie = movieService.GetMovieById(id);
        if (movie == null) return Results.NotFound(new { message = "Film niet gevonden." });

        // Alles wordt overschreven zonder if-controles
        movie.Title = dto.Title;
        movie.ReleaseYear = dto.ReleaseYear;
        movie.GenreId = dto.GenreId;

        movieService.UpdateMovie(movie);
        return Results.NoContent();
    }


}