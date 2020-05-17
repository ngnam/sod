using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StoreOrder.WebApplication.Data.Migrations.Logging
{
    public partial class AddLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "logging");

            migrationBuilder.CreateTable(
                name: "logs",
                schema: "logging",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message = table.Column<string>(type: "text", nullable: true),
                    message_template = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<string>(maxLength: 128, nullable: true),
                    time_stamp = table.Column<DateTimeOffset>(nullable: false),
                    exception = table.Column<string>(type: "text", nullable: true),
                    log_event = table.Column<string>(type: "jsonb", nullable: true),
                    properties = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs",
                schema: "logging");
        }
    }
}
