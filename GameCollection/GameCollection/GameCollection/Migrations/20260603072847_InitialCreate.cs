using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameCollection.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Adventure" },
                    { 3, "RPG" },
                    { 4, "Sports" },
                    { 5, "Strategy" },
                    { 6, "Simulation" },
                    { 7, "Horror" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "GenreId", "Platform", "Rating", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 1, 3, "PC", 10, 2015, "The Witcher 3: Wild Hunt" },
                    { 2, 1, "PlayStation 5", 9, 2022, "Elden Ring" },
                    { 3, 6, "PC", 9, 2016, "Stardew Valley" },
                    { 4, 4, "PlayStation 5", 7, 2023, "FIFA 24" },
                    { 5, 5, "PC", 8, 2016, "Civilization VI" },
                    { 6, 7, "PlayStation 5", 8, 2021, "Resident Evil Village" },
                    { 7, 2, "Nintendo Switch", 10, 2017, "Zelda: Breath of the Wild" },
                    { 8, 1, "Xbox Series X", 8, 2021, "Halo Infinite" },
                    { 9, 5, "Mobile", 7, 2018, "Among Us" },
                    { 10, 1, "PlayStation 4", 10, 2018, "God of War" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GenreId",
                table: "Games",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
