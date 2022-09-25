namespace MoneyFox.Persistence.Migrations;

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

[SuppressMessage("Minor Code Smell", "S1192:String literals should not be duplicated")]
[SuppressMessage("Minor Code Smell", "S3254:Default parameter values should not be passed as arguments")]
public partial class AddCreationDateField : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<DateTime>("CreationTime",
                                             "RecurringPayments",
                                             nullable: false,
                                             defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>("CreationTime",
                                             "Payments",
                                             nullable: false,
                                             defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>("CreationTime",
                                             "Categories",
                                             nullable: false,
                                             defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>("CreationTime",
                                             "Accounts",
                                             nullable: false,
                                             defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn("CreationTime",
                                    "RecurringPayments");

        _ = migrationBuilder.DropColumn("CreationTime",
                                    "Payments");

        _ = migrationBuilder.DropColumn("CreationTime",
                                    "Categories");

        _ = migrationBuilder.DropColumn("CreationTime",
                                    "Accounts");
    }
}
