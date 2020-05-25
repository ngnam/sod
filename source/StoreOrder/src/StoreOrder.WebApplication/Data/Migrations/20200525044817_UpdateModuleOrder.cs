using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class UpdateModuleOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Quality",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Order",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                schema: "Order",
                table: "Orders",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateOn",
                schema: "Order",
                table: "Orders",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "Order",
                table: "OrderDetails",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionDescription",
                schema: "Order",
                table: "OrderDetails",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "OptionId_OptionValueIds",
                schema: "Order",
                table: "OrderDetails",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "Order",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                schema: "Order",
                table: "OrderDetails",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductOrderStatus",
                schema: "Order",
                table: "OrderDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TableName",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdateOn",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OptionDescription",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OptionId_OptionValueIds",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductOrderStatus",
                schema: "Order",
                table: "OrderDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Order",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "Order",
                table: "OrderDetails",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quality",
                schema: "Order",
                table: "OrderDetails",
                type: "integer",
                nullable: true);
        }
    }
}
