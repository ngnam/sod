using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class AddRelationshipCategoryProductWithStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoreId",
                schema: "Product",
                table: "CategoryProducts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_StoreId",
                schema: "Product",
                table: "CategoryProducts",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_Stores_StoreId",
                schema: "Product",
                table: "CategoryProducts",
                column: "StoreId",
                principalSchema: "Store",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_Stores_StoreId",
                schema: "Product",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_StoreId",
                schema: "Product",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "Product",
                table: "CategoryProducts");
        }
    }
}
