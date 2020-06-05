using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class UserDetailsAddColumnGAvartar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GAvartar",
                schema: "Account",
                table: "UserDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GAvartar",
                schema: "Account",
                table: "UserDetails");
        }
    }
}
