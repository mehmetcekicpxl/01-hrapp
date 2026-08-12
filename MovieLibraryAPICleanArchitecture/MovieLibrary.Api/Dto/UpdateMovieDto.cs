namespace MovieLibrary.Api.Dto
{
    public class UpdateMovieDto
    {// Gebruik DTO's om te voorkomen dat Swagger onnodige velden zoals Id vraagt.
        public string Title { get; set; }= string.Empty;
        public int ReleaseYear { get; set; }
        public int  GenreId { get; set; }
    }
}
