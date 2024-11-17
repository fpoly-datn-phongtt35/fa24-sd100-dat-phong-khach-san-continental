using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalRoomPriceTotalServicePriceRoomBookingandaddQuantityinServiceOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ServiceOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRoomPrice",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalServicePrice",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ServiceOrderDetail");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "TotalRoomPrice",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "TotalServicePrice",
                table: "RoomBooking");
        }
    }
}
