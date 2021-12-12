using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemesProject.Data.Migrations
{
    public partial class RenameColumnUserAccountRegisterDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data_Zalozenia_Konta",
                table: "AspNetUsers",
                newName: "Account_Register_Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Account_Register_Date",
                table: "AspNetUsers",
                newName: "Data_Zalozenia_Konta");
        }
    }
}
