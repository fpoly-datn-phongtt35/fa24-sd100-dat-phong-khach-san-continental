using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class fixfeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_RoomBookingDetailId",
                table: "FeedBack");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_RoomBookingDetailId",
                table: "FeedBack",
                column: "RoomBookingDetailId",
                principalTable: "RoomBookingDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_RoomBookingDetailId",
                table: "FeedBack");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_RoomBookingDetailId",
                table: "FeedBack",
                column: "RoomBookingDetailId",
                principalTable: "RoomBookingDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
