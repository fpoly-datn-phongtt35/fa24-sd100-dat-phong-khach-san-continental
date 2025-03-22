using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class mistake1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_Id",
                table: "FeedBack");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBack_RoomBookingDetailId",
                table: "FeedBack",
                column: "RoomBookingDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_RoomBookingDetailId",
                table: "FeedBack",
                column: "RoomBookingDetailId",
                principalTable: "RoomBookingDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_RoomBookingDetailId",
                table: "FeedBack");

            migrationBuilder.DropIndex(
                name: "IX_FeedBack_RoomBookingDetailId",
                table: "FeedBack");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_RoomBookingDetail_Id",
                table: "FeedBack",
                column: "Id",
                principalTable: "RoomBookingDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
