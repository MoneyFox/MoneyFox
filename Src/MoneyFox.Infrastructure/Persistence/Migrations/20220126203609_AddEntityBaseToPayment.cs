namespace MoneyFox.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddEntityBaseToPayment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<DateTime>(
            name: "Created",
            table: "Payments",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "LastModified",
            table: "Payments",
            nullable: true);

        _ = migrationBuilder.Sql("Update Payments Set Created = CreationTime, LastModified = ModificationDate");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "Created",
            table: "Payments");

        _ = migrationBuilder.DropColumn(
            name: "LastModified",
            table: "Payments");
    }
}
