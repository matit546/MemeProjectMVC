using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesProject.Data.Migrations
{
    public partial class LikeFavouriteMeme4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritesMemes",
                columns: table => new
                {
                    IdFavoritesMemes = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMeme = table.Column<long>(type: "bigint", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesMemes", x => x.IdFavoritesMemes);
                    table.ForeignKey(
                        name: "FK_FavoritesMemes_Memes_IdMeme",
                        column: x => x.IdMeme,
                        principalTable: "Memes",
                        principalColumn: "IdMeme",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikedMemes",
                columns: table => new
                {
                    IdLikedMemes = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMeme = table.Column<long>(type: "bigint", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedMemes", x => x.IdLikedMemes);
                    table.ForeignKey(
                        name: "FK_LikedMemes_Memes_IdMeme",
                        column: x => x.IdMeme,
                        principalTable: "Memes",
                        principalColumn: "IdMeme",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritesMemes_IdMeme",
                table: "FavoritesMemes",
                column: "IdMeme");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMemes_IdMeme",
                table: "LikedMemes",
                column: "IdMeme");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritesMemes");

            migrationBuilder.DropTable(
                name: "LikedMemes");
        }
    }
}
