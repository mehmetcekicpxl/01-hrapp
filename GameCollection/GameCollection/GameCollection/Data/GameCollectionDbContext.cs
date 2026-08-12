using GameCollection.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollection.Data;

public class GameCollectionDbContext : DbContext
{
    public GameCollectionDbContext(DbContextOptions<GameCollectionDbContext> options)
        : base(options) { }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "RPG" },
            new Genre { Id = 4, Name = "Sports" },
            new Genre { Id = 5, Name = "Strategy" },
            new Genre { Id = 6, Name = "Simulation" },
            new Genre { Id = 7, Name = "Horror" }
        );

        modelBuilder.Entity<Game>().HasData(
            new Game { Id = 1, Title = "The Witcher 3: Wild Hunt", Platform = "PC", GenreId = 3, ReleaseYear = 2015, Rating = 10 },
            new Game { Id = 2, Title = "Elden Ring", Platform = "PlayStation 5", GenreId = 1, ReleaseYear = 2022, Rating = 9 },
            new Game { Id = 3, Title = "Stardew Valley", Platform = "PC", GenreId = 6, ReleaseYear = 2016, Rating = 9 },
            new Game { Id = 4, Title = "FIFA 24", Platform = "PlayStation 5", GenreId = 4, ReleaseYear = 2023, Rating = 7 },
            new Game { Id = 5, Title = "Civilization VI", Platform = "PC", GenreId = 5, ReleaseYear = 2016, Rating = 8 },
            new Game { Id = 6, Title = "Resident Evil Village", Platform = "PlayStation 5", GenreId = 7, ReleaseYear = 2021, Rating = 8 },
            new Game { Id = 7, Title = "Zelda: Breath of the Wild", Platform = "Nintendo Switch", GenreId = 2, ReleaseYear = 2017, Rating = 10 },
            new Game { Id = 8, Title = "Halo Infinite", Platform = "Xbox Series X", GenreId = 1, ReleaseYear = 2021, Rating = 8 },
            new Game { Id = 9, Title = "Among Us", Platform = "Mobile", GenreId = 5, ReleaseYear = 2018, Rating = 7 },
            new Game { Id = 10, Title = "God of War", Platform = "PlayStation 4", GenreId = 1, ReleaseYear = 2018, Rating = 10 }
        );
    }
}