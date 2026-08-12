using Microsoft.EntityFrameworkCore;
using MovieLibrary.Api.Endpoints;
using MovieLibrary.Application.Repositories;
using MovieLibrary.Application.Services;
using MovieLibrary.Infrastructure.Data;
using MovieLibrary.Infrastructure.Repositories;

namespace MovieLibrary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // toevoegen van services aan de DI-container swagger en database context
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<MovieLibraryDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("MovieLibraryConnection"));
            }); 
            
            

            //toevoegen van repositories en services aan de DI-container
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<IMovieService, MovieService>();


            var app = builder.Build();


            app.UseSwagger();
            app.UseSwaggerUI();


            app.MapGenreEndpoints();
            app.MapMovieEndpoints();

            app.Run();
        }
    }
}
