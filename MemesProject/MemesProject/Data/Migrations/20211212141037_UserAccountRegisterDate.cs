using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesProject.Data.Migrations
{
    public partial class UserAccountRegisterDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data_Zalozenia_Konta",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data_Zalozenia_Konta",
                table: "AspNetUsers");
        }
    }
}
