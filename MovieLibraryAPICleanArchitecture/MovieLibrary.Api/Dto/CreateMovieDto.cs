namespace MovieLibrary.Api.Dto
{
    public class CreateMovieDto
    {
        // Id yok! (Veritabanı verecek)
        // Genre objesi yok! (Sadece id'sini almamız yeterli)

        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int GenreId { get; set; } // Sadece hangi kategoriye ait olduğunu bilsek yeter
    }
}
