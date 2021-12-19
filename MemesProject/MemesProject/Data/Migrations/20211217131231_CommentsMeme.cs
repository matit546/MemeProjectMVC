using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesProject.Data.Migrations
{
    public partial class CommentsMeme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memes_IdCategory",
                table: "Memes");

            migrationBuilder.CreateTable(
                name: "CommentsHubs",
                columns: table => new
                {
                    IdCommentHub = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMeme = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsHubs", x => x.IdCommentHub);
                    table.ForeignKey(
                        name: "FK_CommentsHubs_Memes_IdMeme",
                        column: x => x.IdMeme,
                        principalTable: "Memes",
                        principalColumn: "IdMeme",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    IdComment = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IfBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    IdCommentsHub = table.Column<long>(type: "bigint", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.IdComment);
                    table.ForeignKey(
                        name: "FK_Comments_CommentsHubs_IdCommentsHub",
                        column: x => x.IdCommentsHub,
                        principalTable: "CommentsHubs",
                        principalColumn: "IdCommentHub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memes_IdCategory",
                table: "Memes",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdCommentsHub",
                table: "Comments",
                column: "IdCommentsHub");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsHubs_IdMeme",
                table: "CommentsHubs",
                column: "IdMeme");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CommentsHubs");

            migrationBuilder.DropIndex(
                name: "IX_Memes_IdCategory",
                table: "Memes");

            migrationBuilder.CreateIndex(
                name: "IX_Memes_IdCategory",
                table: "Memes",
                column: "IdCategory",
                unique: true);
        }
    }
}
