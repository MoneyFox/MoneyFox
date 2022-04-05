using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.Persistence.Migrations
{
    public partial class ReaddRemovedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecurringPayments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "RecurringPayments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Payments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Payments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Categories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOverdrawn",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Accounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.Sql("Update Payments Set CreationTime = Created , ModificationDate= LastModified");
            migrationBuilder.Sql("Update RecurringPayments Set CreationTime=Created, ModificationDate=LastModified");
            migrationBuilder.Sql("Update Categories Set CreationTime=Created , ModificationDate=LastModified");
            migrationBuilder.Sql("Update Accounts Set CreationTime=Created, ModificationDate=LastModified");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecurringPayments");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "RecurringPayments");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsOverdrawn",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Accounts");
        }
    }
}
