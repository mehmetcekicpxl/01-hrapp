namespace MovieLibrary.Api.Dto
{
    public class UpdateGenreDto
    {// Gebruik DTO's om te voorkomen dat Swagger onnodige velden zoals Id vraagt.
        public string Name { get; set; } = string.Empty;
    }
}
