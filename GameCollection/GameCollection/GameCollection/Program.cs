using GameCollection.Data;
using GameCollection.Repositories;
using GameCollection.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<GameCollectionDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GameCollectionConnection")));

builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<GenreRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();