using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace MoneyFox.Core.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentBalance = table.Column<double>(nullable: false),
                    Iban = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<double>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    ChargedAccountId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsCleared = table.Column<bool>(nullable: false),
                    IsRecurring = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    RecurringPaymentId = table.Column<int>(nullable: false),
                    TargetAccountId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "RecurringPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<double>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    ChargedAccountId = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsEndless = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Recurrence = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    TargetAccountId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringPayment", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Accounts");
            migrationBuilder.DropTable("Categories");
            migrationBuilder.DropTable("Payments");
            migrationBuilder.DropTable("RecurringPayments");
        }
    }
}
