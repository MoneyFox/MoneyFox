using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.DataLayer.Migrations
{
    public partial class AddModificationDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "RecurringPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Categories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "RecurringPayments");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Accounts");
        }
    }
}
