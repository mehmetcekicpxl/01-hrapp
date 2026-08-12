using GameCollection.Data;
using GameCollection.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollection.Repositories;

public class GenreRepository
{
    private readonly IDbContextFactory<GameCollectionDbContext> _contextFactory;

    public GenreRepository(IDbContextFactory<GameCollectionDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IEnumerable<Genre> GetAll()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Genres.OrderBy(g => g.Name).ToList();
    }
}