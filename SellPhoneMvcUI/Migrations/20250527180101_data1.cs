using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellPhoneMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class data1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "OrderStatuses",
            columns: new[] { "StatusId", "StatusName", "Description" },
            values: new object[,]
            {
                { 1, "Pending", "Order is pending" },
                { 2, "Processing", "Order is being processed" },
                { 3, "Completed", "Order is completed" },
                { 4, "Cancelled", "Order is cancelled" }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
