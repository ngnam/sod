using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class AddProductDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductDetails",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortDescription = table.Column<string>(maxLength: 250, nullable: true),
                    LongDescription = table.Column<string>(maxLength: 500, nullable: true),
                    ImageThumb = table.Column<string>(maxLength: 500, nullable: true),
                    ImageWidthThumb = table.Column<int>(nullable: true),
                    ImageHeightThumb = table.Column<int>(nullable: true),
                    ImageOrigin = table.Column<string>(maxLength: 500, nullable: true),
                    ImageAlbumJson = table.Column<string>(type: "TEXT", nullable: true),
                    ProductId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageThumb = table.Column<string>(maxLength: 500, nullable: true),
                    ImageWidthThumb = table.Column<int>(nullable: true),
                    ImageHeightThumb = table.Column<int>(nullable: true),
                    ImageOrigin = table.Column<string>(maxLength: 500, nullable: true),
                    ProductDetailId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalSchema: "Product",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Id",
                schema: "Product",
                table: "ProductDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                schema: "Product",
                table: "ProductDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductDetailId",
                schema: "Product",
                table: "ProductImages",
                column: "ProductDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "ProductDetails",
                schema: "Product");
        }
    }
}
