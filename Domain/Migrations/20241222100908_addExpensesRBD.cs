using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addExpensesRBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Expenses",
                table: "RoomBookingDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "RoomBookingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingBy",
                table: "RoomBooking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExpenses",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPriceReality",
                table: "RoomBooking",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CheckOutTime",
                table: "ResidenceRegistration",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expenses",
                table: "RoomBookingDetail");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "RoomBookingDetail");

            migrationBuilder.DropColumn(
                name: "BookingBy",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "TotalExpenses",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "TotalPriceReality",
                table: "RoomBooking");
        }
    }
}
