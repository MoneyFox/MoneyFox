namespace MoneyFox.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Minor Code Smell", "S1192:String literals should not be duplicated")]
    [SuppressMessage("Minor Code Smell", "S3254:Default parameter values should not be passed as arguments")]
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
