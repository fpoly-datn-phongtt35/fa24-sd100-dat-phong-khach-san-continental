using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class xoaCungTamTru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ResidenceRegistration");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "ResidenceRegistration");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ResidenceRegistration");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ResidenceRegistration");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "ResidenceRegistration");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ResidenceRegistration");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "ResidenceRegistration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ResidenceRegistration",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "ResidenceRegistration",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ResidenceRegistration",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "ResidenceRegistration",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "ResidenceRegistration",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "ResidenceRegistration",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "ResidenceRegistration",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
