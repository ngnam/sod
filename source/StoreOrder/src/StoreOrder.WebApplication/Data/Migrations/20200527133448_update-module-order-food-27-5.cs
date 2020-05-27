using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class updatemoduleorderfood275 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCookingOrders",
                schema: "Order");

            migrationBuilder.DropColumn(
                name: "ProductOrderStatus",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "UserCookingId",
                schema: "Order",
                table: "Orders",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Order",
                table: "OrderDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCookingId",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductOrderStatus",
                schema: "Order",
                table: "OrderDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCookingOrders",
                schema: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreateOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DurationTime = table.Column<int>(type: "integer", nullable: true),
                    OrderId = table.Column<string>(type: "character varying(50)", nullable: true),
                    StatusCooking = table.Column<int>(type: "integer", nullable: true),
                    UpdateOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCookingOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCookingOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCookingOrders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCookingOrders_OrderId",
                schema: "Order",
                table: "UserCookingOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCookingOrders_UserId",
                schema: "Order",
                table: "UserCookingOrders",
                column: "UserId");
        }
    }
}
