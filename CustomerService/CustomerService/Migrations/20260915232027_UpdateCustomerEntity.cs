using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "customer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "customer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                table: "customer",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KycStatus",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                table: "customer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "customer",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "KycStatus",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "customer");
        }
    }
}
