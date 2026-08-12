using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Domain.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }

    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}
