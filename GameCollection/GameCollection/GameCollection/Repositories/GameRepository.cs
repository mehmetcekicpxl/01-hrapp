using GameCollection.Data;
using GameCollection.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollection.Repositories;

public class GameRepository
{
    private readonly IDbContextFactory<GameCollectionDbContext> _contextFactory;

    public GameRepository(IDbContextFactory<GameCollectionDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IEnumerable<Game> GetAll()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Games
            .Include(g => g.Genre)
            .OrderBy(g => g.Title)
            .ToList();
    }

    public Game? GetById(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Games
            .Include(g => g.Genre)
            .FirstOrDefault(g => g.Id == id);
    }

    public void Add(Game game)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Games.Add(game);
        context.SaveChanges();
    }

    public void Update(Game game)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Games.Update(game);
        context.SaveChanges();
    }

    public void Delete(Game game)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Games.Remove(game);
        context.SaveChanges();
    }
}