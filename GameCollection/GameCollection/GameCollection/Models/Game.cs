namespace GameCollection.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
    public int ReleaseYear { get; set; }
    public int? Rating { get; set; }
}