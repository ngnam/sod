using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class AddRelationshipStoreWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Store",
                table: "Stores");

            migrationBuilder.AddColumn<string>(
                name: "CreateByUserId",
                schema: "Store",
                table: "Stores",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreId",
                schema: "Account",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StoreId",
                schema: "Account",
                table: "Users",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "Account",
                table: "Users",
                column: "StoreId",
                principalSchema: "Store",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Stores_StoreId",
                schema: "Account",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_StoreId",
                schema: "Account",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreateByUserId",
                schema: "Store",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "Account",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Store",
                table: "Stores",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
