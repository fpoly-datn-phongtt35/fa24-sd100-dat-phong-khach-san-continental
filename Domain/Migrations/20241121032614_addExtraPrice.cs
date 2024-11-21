using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addExtraPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExtraPrice",
                table: "ServiceOrderDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraPrice",
                table: "RoomBookingDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalServicePrice",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExtraPrice",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraPrice",
                table: "ServiceOrderDetail");

            migrationBuilder.DropColumn(
                name: "ExtraPrice",
                table: "RoomBookingDetail");

            migrationBuilder.DropColumn(
                name: "TotalExtraPrice",
                table: "RoomBooking");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalServicePrice",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
