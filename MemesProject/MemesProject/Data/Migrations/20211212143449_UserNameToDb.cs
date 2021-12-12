using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesProject.Data.Migrations
{
    public partial class UserNameToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RealUserName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealUserName",
                table: "AspNetUsers");
        }
    }
}
