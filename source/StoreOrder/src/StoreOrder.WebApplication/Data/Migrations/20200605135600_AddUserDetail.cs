using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class AddUserDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SlugCode",
                schema: "Store",
                table: "CategoryStores",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserDetails",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProvideId = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(maxLength: 550, nullable: true),
                    AddRess2 = table.Column<string>(maxLength: 550, nullable: true),
                    AddRess3 = table.Column<string>(maxLength: 550, nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetails_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                schema: "Account",
                table: "UserDetails",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDetails",
                schema: "Account");

            migrationBuilder.DropColumn(
                name: "SlugCode",
                schema: "Store",
                table: "CategoryStores");
        }
    }
}
