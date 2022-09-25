namespace MoneyFox.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddEntityBaseToRecurringPayment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<DateTime>(
            name: "Created",
            table: "RecurringPayments",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "LastModified",
            table: "RecurringPayments",
            nullable: true);

        _ = migrationBuilder.Sql("Update RecurringPayments Set Created = CreationTime, LastModified = ModificationDate");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "Created",
            table: "RecurringPayments");

        _ = migrationBuilder.DropColumn(
            name: "LastModified",
            table: "RecurringPayments");
    }
}
