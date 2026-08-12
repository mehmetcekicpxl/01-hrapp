using System.ComponentModel.DataAnnotations;

namespace GameCollection.Models;

public class GameModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Platform is required.")]
    public string Platform { get; set; } = string.Empty;

    [Required(ErrorMessage = "Genre is required.")]
    public int? GenreId { get; set; }

    [Required(ErrorMessage = "Release year is required.")]
    [Range(1970, 2030, ErrorMessage = "Release year must be between 1970 and 2030.")]
    public int ReleaseYear { get; set; } = DateTime.Today.Year;

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int? Rating { get; set; }
}