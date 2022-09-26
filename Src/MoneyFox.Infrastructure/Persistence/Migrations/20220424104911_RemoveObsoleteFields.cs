namespace MoneyFox.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class RemoveObsoleteFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "RecurringPayments");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "RecurringPayments");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "Payments");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "Payments");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "Categories");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "Categories");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "Accounts");

        _ = migrationBuilder.DropColumn(
            name: "IsOverdrawn",
            table: "Accounts");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "Accounts");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "RecurringPayments",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "RecurringPayments",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "Payments",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "Payments",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "Categories",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "Categories",
            type: "TEXT",
            nullable: true);

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "Accounts",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<bool>(
            name: "IsOverdrawn",
            table: "Accounts",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "Accounts",
            type: "TEXT",
            nullable: true);
    }
}
