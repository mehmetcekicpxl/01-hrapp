namespace MovieLibrary.Api.Dto
{
    public class PatchMovieDto
    {
        public string? Title { get; set; }
        public int? ReleaseYear { get; set; }
        public int? GenreId { get; set; }
    }
}
