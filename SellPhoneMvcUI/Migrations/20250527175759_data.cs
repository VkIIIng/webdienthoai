using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SellPhoneMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorys",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Electronics", "Devices and gadgets" },
                    { 2, "Accessories", "Phone accessories" },
                    { 3, "Wearables", "Smartwatches and fitness trackers" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Brand", "CategoryId", "CreatedAt", "Description", "ImageUrl", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "BrandA", 1, new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest smartphone with advanced features", "https://images.samsung.com/is/image/samsung/p5/vn/smartphones/SM-A605FN_021_Front_Lavender.png?$ORIGIN_PNG$", 699.99m, "Smartphone X", 50 },
                    { 2, "BrandB", 1, new DateTime(2025, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "High-performance tablet", "https://storage.micromagma.ma/micromagma/0cb7a714-d39d-4f30-958b-d652349dca33.png", 499.99m, "Tablet Pro", 30 },
                    { 3, "BrandC", 2, new DateTime(2025, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Durable phone case", "https://hoanghamobile.com/Uploads/2023/12/14/a15-lte-3.png", 19.99m, "Phone Case", 100 },
                    { 4, "BrandD", 3, new DateTime(2025, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fitness tracking smartwatch", "https://th.bing.com/th/id/OIP.qd_zK1LlhLDNXbGLCoR77wHaHa?rs=1&pid=ImgDetMain", 199.99m, "Smartwatch Z", 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorys",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorys",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorys",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test", x => x.Id);
                });
        }
    }
}
