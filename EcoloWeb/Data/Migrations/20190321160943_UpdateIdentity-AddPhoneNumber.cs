using Microsoft.EntityFrameworkCore.Migrations;

namespace EcoloWeb.Data.Migrations
{
    public partial class UpdateIdentityAddPhoneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumer",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumer",
                table: "AspNetUsers");
        }
    }
}
