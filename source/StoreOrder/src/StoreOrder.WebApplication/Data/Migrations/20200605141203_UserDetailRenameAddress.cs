using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class UserDetailRenameAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddRess3",
                schema: "Account",
                table: "UserDetails",
                newName: "Address3");

            migrationBuilder.RenameColumn(
                name: "AddRess2",
                schema: "Account",
                table: "UserDetails",
                newName: "Address2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address3",
                schema: "Account",
                table: "UserDetails",
                newName: "AddRess3");

            migrationBuilder.RenameColumn(
                name: "Address2",
                schema: "Account",
                table: "UserDetails",
                newName: "AddRess2");
        }
    }
}
