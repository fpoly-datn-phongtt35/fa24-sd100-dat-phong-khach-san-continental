using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class delete_ServiceOrder_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderDetail_ServiceOrder_ServiceOrderId",
                table: "ServiceOrderDetail");

            migrationBuilder.DropTable(
                name: "ServiceOrder");

            migrationBuilder.RenameColumn(
                name: "ServiceOrderId",
                table: "ServiceOrderDetail",
                newName: "RoomBookingId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderDetail_ServiceOrderId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderDetail_RoomBooking_RoomBookingId",
                table: "ServiceOrderDetail");

            migrationBuilder.RenameColumn(
                name: "RoomBookingId",
                table: "ServiceOrderDetail",
                newName: "ServiceOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderDetail_RoomBookingId",
                table: "ServiceOrderDetail",
                newName: "IX_ServiceOrderDetail_ServiceOrderId");

            migrationBuilder.CreateTable(
                name: "ServiceOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RoomBookingDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceOrder_RoomBookingDetail_RoomBookingDetailId",
                        column: x => x.RoomBookingDetailId,
                        principalTable: "RoomBookingDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceOrder_RoomBooking_RoomBookingId",
                        column: x => x.RoomBookingId,
                        principalTable: "RoomBooking",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_RoomBookingDetailId",
                table: "ServiceOrder",
                column: "RoomBookingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_RoomBookingId",
                table: "ServiceOrder",
                column: "RoomBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderDetail_ServiceOrder_ServiceOrderId",
                table: "ServiceOrderDetail",
                column: "ServiceOrderId",
                principalTable: "ServiceOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
