using Microsoft.EntityFrameworkCore.Migrations;
using System;

#pragma warning disable S3254 // Default parameter values should not be passed as arguments
#pragma warning disable S3900 // Arguments of public methods should be validated against null
#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable S1192 // String literals should not be duplicated
namespace MoneyFox.Persistence.Migrations
{
    public partial class AddCreationDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("CreationTime",
                                                 "RecurringPayments",
                                                 nullable: false,
                                                 defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>("CreationTime",
                                                 "Payments",
                                                 nullable: false,
                                                 defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>("CreationTime",
                                                 "Categories",
                                                 nullable: false,
                                                 defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>("CreationTime",
                                                 "Accounts",
                                                 nullable: false,
                                                 defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("CreationTime",
                                        "RecurringPayments");

            migrationBuilder.DropColumn("CreationTime",
                                        "Payments");

            migrationBuilder.DropColumn("CreationTime",
                                        "Categories");

            migrationBuilder.DropColumn("CreationTime",
                                        "Accounts");
        }
    }
}
