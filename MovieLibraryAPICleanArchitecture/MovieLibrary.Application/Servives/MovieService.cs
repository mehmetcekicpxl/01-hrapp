using MovieLibrary.Application.Repositories;
using MovieLibrary.Domain.Entities;

namespace MovieLibrary.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        return _movieRepository.GetAll();
    }

    public Movie? GetMovieById(int id)
    {
        return _movieRepository.GetById(id);
    }

    public void AddMovie(Movie movie)
    {
        _movieRepository.Add(movie);
        _movieRepository.SaveChanges();
    }

    public bool DeleteMovie(int id)
    {
        var movie = _movieRepository.GetById(id);

        if (movie == null)
        {
            return false;
        }

        _movieRepository.Delete(movie);
        _movieRepository.SaveChanges();
        return true;
    }
    public void UpdateMovie(Movie movie)
    {         var existingMovie = _movieRepository.GetById(movie.Id);
        if (existingMovie == null)
        {
            throw new Exception($"Movie with id {movie.Id} not found.");
        }
        // Update de eigenschappen van de bestaande film
        existingMovie.Title = movie.Title;
        existingMovie.ReleaseYear = movie.ReleaseYear;
        existingMovie.GenreId = movie.GenreId;
        _movieRepository.SaveChanges();
    }
}