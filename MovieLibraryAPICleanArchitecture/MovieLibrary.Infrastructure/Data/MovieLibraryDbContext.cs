using Microsoft.EntityFrameworkCore;
using MovieLibrary.Domain.Entities;

namespace MovieLibrary.Infrastructure.Data;

public class MovieLibraryDbContext : DbContext
{
    public MovieLibraryDbContext(DbContextOptions<MovieLibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
}