using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class _51 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderDetail_RoomBooking_RoomBookingId",
                table: "ServiceOrderDetail");

            migrationBuilder.RenameColumn(
                name: "RoomBookingId",
                table: "ServiceOrderDetail",
                newName: "RoomBookingDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderDetail_RoomBookingId",
                table: "ServiceOrderDetail",
                newName: "IX_ServiceOrderDetail_RoomBookingDetailId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CheckInTime",
                table: "ResidenceRegistration",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderDetail_RoomBookingDetail_RoomBookingDetailId",
                table: "ServiceOrderDetail",
                column: "RoomBookingDetailId",
                principalTable: "RoomBookingDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderDetail_RoomBookingDetail_RoomBookingDetailId",
                table: "ServiceOrderDetail");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "ResidenceRegistration");

            migrationBuilder.RenameColumn(
                name: "RoomBookingDetailId",
                table: "ServiceOrderDetail",
                newName: "RoomBookingId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderDetail_RoomBookingDetailId",
                table: "ServiceOrderDetail",
                newName: "IX_ServiceOrderDetail_RoomBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderDetail_RoomBooking_RoomBookingId",
                table: "ServiceOrderDetail",
                column: "RoomBookingId",
                principalTable: "RoomBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
