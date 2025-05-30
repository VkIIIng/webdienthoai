using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellPhoneMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class ThemStatusIdVaoOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_OrderStatusStatusId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderStatusStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStatusStatusId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_StatusId",
                table: "Orders",
                column: "StatusId",
                principalTable: "OrderStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_StatusId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusStatusId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusStatusId",
                table: "Orders",
                column: "OrderStatusStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_OrderStatusStatusId",
                table: "Orders",
                column: "OrderStatusStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "StatusId");
        }
    }
}
