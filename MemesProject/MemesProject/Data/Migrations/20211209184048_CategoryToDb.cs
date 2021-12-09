using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesProject.Data.Migrations
{
    public partial class CategoryToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IdCategory = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IdCategory);
                });

            migrationBuilder.CreateTable(
                name: "Memes",
                columns: table => new
                {
                    IdMeme = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAlt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    IfBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IfApproved = table.Column<bool>(type: "bit", nullable: false),
                    IdCategory = table.Column<long>(type: "bigint", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memes", x => x.IdMeme);
                    table.ForeignKey(
                        name: "FK_Memes_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memes_IdCategory",
                table: "Memes",
                column: "IdCategory",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memes");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
