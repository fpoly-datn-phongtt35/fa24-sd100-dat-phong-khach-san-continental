using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixFeedbackTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_Customer_CustomerId",
                table: "FeedBack");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_RoomBooking_Id",
                table: "FeedBack");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_Staff_StaffId",
                table: "FeedBack");

            migrationBuilder.DropIndex(
                name: "IX_FeedBack_CustomerId",
                table: "FeedBack");

            migrationBuilder.DropIndex(
                name: "IX_FeedBack_StaffId",
                table: "FeedBack");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "FeedBack");

            migrationBuilder.DropColumn(
                name: "RoomBookingId",
                table: "FeedBack");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "FeedBack",
                newName: "RoomBookingDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_Id",
                table: "FeedBack",
                column: "Id",
                principalTable: "RoomBookingDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_Id",
                table: "FeedBack");

            migrationBuilder.RenameColumn(
                name: "RoomBookingDetailId",
                table: "FeedBack",
                newName: "StaffId");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "FeedBack",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomBookingId",
                table: "FeedBack",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FeedBack_CustomerId",
                table: "FeedBack",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBack_StaffId",
                table: "FeedBack",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_Customer_CustomerId",
                table: "FeedBack",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_RoomBooking_Id",
                table: "FeedBack",
                column: "Id",
                principalTable: "RoomBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_Staff_StaffId",
                table: "FeedBack",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
