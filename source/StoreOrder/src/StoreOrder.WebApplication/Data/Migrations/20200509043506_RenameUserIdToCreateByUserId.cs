using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class RenameUserIdToCreateByUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserId",
                schema: "Product",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserId",
                schema: "Product",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Product",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "CreateByUserId",
                schema: "Product",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreateByUserId",
                schema: "Product",
                table: "Products",
                column: "CreateByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_CreateByUserId",
                schema: "Product",
                table: "Products",
                column: "CreateByUserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_CreateByUserId",
                schema: "Product",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreateByUserId",
                schema: "Product",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreateByUserId",
                schema: "Product",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Product",
                table: "Products",
                type: "character varying(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                schema: "Product",
                table: "Products",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserId",
                schema: "Product",
                table: "Products",
                column: "UserId",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
