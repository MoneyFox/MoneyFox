using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable S3254 // Default parameter values should not be passed as arguments
#pragma warning disable S3900 // Arguments of public methods should be validated against null
#pragma warning disable CA1062 // Validate arguments of public methods
namespace MoneyFox.Persistence.Migrations
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1192:String literals should not be duplicated", Justification = "<Pending>")]
    public partial class AddModificationDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                "ModificationDate",
                "RecurringPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                "ModificationDate",
                "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                "ModificationDate",
                "Categories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                "ModificationDate",
                "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ModificationDate",
                "RecurringPayments");

            migrationBuilder.DropColumn(
                "ModificationDate",
                "Payments");

            migrationBuilder.DropColumn(
                "ModificationDate",
                "Categories");

            migrationBuilder.DropColumn(
                "ModificationDate",
                "Accounts");
        }
    }
}
