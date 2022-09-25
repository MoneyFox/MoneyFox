namespace MoneyFox.Persistence.Migrations;

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

[SuppressMessage("Major Code Smell", "S138:Functions should not have too many lines of code", Justification = "Generated Code")]
[SuppressMessage("Minor Code Smell", "S1192:String literals should not be duplicated")]
public partial class InitMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable("Accounts",
                                     table => new
                                     {
                                         Id = table.Column<int>()
                                                            .Annotation("Sqlite:Autoincrement", true),
                                         CurrentBalance = table.Column<double>(),
                                         Iban = table.Column<string>(nullable: true),
                                         IsExcluded = table.Column<bool>(),
                                         IsOverdrawn = table.Column<bool>(),
                                         Name = table.Column<string>(),
                                         Note = table.Column<string>(nullable: true)
                                     },
                                     constraints: table => table.PrimaryKey("PK_Accounts", x => x.Id));

        _ = migrationBuilder.CreateTable("Categories",
                                     table => new
                                     {
                                         Id = table.Column<int>()
                                                            .Annotation("Sqlite:Autoincrement", true),
                                         Name = table.Column<string>(),
                                         Note = table.Column<string>(nullable: true)
                                     },
                                     constraints: table => table.PrimaryKey("PK_Categories", x => x.Id));

        _ = migrationBuilder.CreateTable("RecurringPayments",
                                     table => new
                                     {
                                         Id = table.Column<int>()
                                                            .Annotation("Sqlite:Autoincrement", true),
                                         Amount = table.Column<double>(),
                                         CategoryId = table.Column<int>(nullable: true),
                                         ChargedAccountId = table.Column<int>(),
                                         EndDate = table.Column<DateTime>(nullable: true),
                                         IsEndless = table.Column<bool>(),
                                         Note = table.Column<string>(nullable: true),
                                         Recurrence = table.Column<int>(),
                                         StartDate = table.Column<DateTime>(),
                                         TargetAccountId = table.Column<int>(nullable: true),
                                         Type = table.Column<int>()
                                     },
                                     constraints: table =>
                                                  {
                                                      _ = table.PrimaryKey("PK_RecurringPayments", x => x.Id);
                                                      _ = table.ForeignKey("FK_RecurringPayments_Categories_CategoryId",
                                                                       x => x.CategoryId,
                                                                       "Categories",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.SetNull);
                                                      _ = table.ForeignKey("FK_RecurringPayments_Accounts_ChargedAccountId",
                                                                       x => x.ChargedAccountId,
                                                                       "Accounts",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.Cascade);
                                                      _ = table.ForeignKey("FK_RecurringPayments_Accounts_TargetAccountId",
                                                                       x => x.TargetAccountId,
                                                                       "Accounts",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.SetNull);
                                                  });

        _ = migrationBuilder.CreateTable("Payments",
                                     table => new
                                     {
                                         Id = table.Column<int>()
                                                            .Annotation("Sqlite:Autoincrement", true),
                                         Amount = table.Column<double>(),
                                         CategoryId = table.Column<int>(nullable: true),
                                         ChargedAccountId = table.Column<int>(),
                                         Date = table.Column<DateTime>(),
                                         IsCleared = table.Column<bool>(),
                                         IsRecurring = table.Column<bool>(),
                                         Note = table.Column<string>(nullable: true),
                                         RecurringPaymentId = table.Column<int>(nullable: true),
                                         TargetAccountId = table.Column<int>(nullable: true),
                                         Type = table.Column<int>()
                                     },
                                     constraints: table =>
                                                  {
                                                      _ = table.PrimaryKey("PK_Payments", x => x.Id);
                                                      _ = table.ForeignKey("FK_Payments_Categories_CategoryId",
                                                                       x => x.CategoryId,
                                                                       "Categories",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.SetNull);
                                                      _ = table.ForeignKey("FK_Payments_Accounts_ChargedAccountId",
                                                                       x => x.ChargedAccountId,
                                                                       "Accounts",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.Cascade);
                                                      _ = table.ForeignKey("FK_Payments_RecurringPayments_RecurringPaymentId",
                                                                       x => x.RecurringPaymentId,
                                                                       "RecurringPayments",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.SetNull);
                                                      _ = table.ForeignKey("FK_Payments_Accounts_TargetAccountId",
                                                                       x => x.TargetAccountId,
                                                                       "Accounts",
                                                                       "Id",
                                                                       onDelete: ReferentialAction.SetNull);
                                                  });

        _ = migrationBuilder.CreateIndex("IX_Accounts_Name",
                                     "Accounts",
                                     "Name");

        _ = migrationBuilder.CreateIndex("IX_Categories_Name",
                                     "Categories",
                                     "Name");

        _ = migrationBuilder.CreateIndex("IX_Payments_CategoryId",
                                     "Payments",
                                     "CategoryId");

        _ = migrationBuilder.CreateIndex("IX_Payments_ChargedAccountId",
                                     "Payments",
                                     "ChargedAccountId");

        _ = migrationBuilder.CreateIndex("IX_Payments_RecurringPaymentId",
                                     "Payments",
                                     "RecurringPaymentId");

        _ = migrationBuilder.CreateIndex("IX_Payments_TargetAccountId",
                                     "Payments",
                                     "TargetAccountId");

        _ = migrationBuilder.CreateIndex("IX_RecurringPayments_CategoryId",
                                     "RecurringPayments",
                                     "CategoryId");

        _ = migrationBuilder.CreateIndex("IX_RecurringPayments_ChargedAccountId",
                                     "RecurringPayments",
                                     "ChargedAccountId");

        _ = migrationBuilder.CreateIndex("IX_RecurringPayments_TargetAccountId",
                                     "RecurringPayments",
                                     "TargetAccountId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
                                   "Payments");

        _ = migrationBuilder.DropTable(
                                   "RecurringPayments");

        _ = migrationBuilder.DropTable(
                                   "Categories");

        _ = migrationBuilder.DropTable(
                                   "Accounts");
    }
}

