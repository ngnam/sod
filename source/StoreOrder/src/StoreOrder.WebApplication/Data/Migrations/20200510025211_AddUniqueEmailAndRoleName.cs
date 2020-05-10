using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class AddUniqueEmailAndRoleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Account",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                schema: "Account",
                table: "Roles",
                column: "RoleName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "Account",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleName",
                schema: "Account",
                table: "Roles");
        }
    }
}
