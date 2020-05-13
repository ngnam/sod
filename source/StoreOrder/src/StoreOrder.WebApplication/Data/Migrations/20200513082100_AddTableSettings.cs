using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class AddTableSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Setting");

            migrationBuilder.CreateTable(
                name: "Settings",
                schema: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SettingKey = table.Column<string>(maxLength: 64, nullable: false),
                    SettingDataType = table.Column<string>(maxLength: 64, nullable: false),
                    SettingValueVarchar = table.Column<string>(maxLength: 1024, nullable: true),
                    SettingDesc = table.Column<string>(maxLength: 500, nullable: true),
                    SettingType = table.Column<int>(nullable: true),
                    SettingGroup = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings",
                schema: "Setting");
        }
    }
}
