using System;
using Microsoft.Data.Entity.Migrations;

namespace MoneyFox.Core.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Account", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                CurrentBalance = table.Column<double>(nullable: false),
                Iban = table.Column<string>(nullable: true),
                Name = table.Column<string>(nullable: false),
                Note = table.Column<string>(nullable: true)
            },
                constraints: table => { table.PrimaryKey("PK_Account", x => x.Id); });
            migrationBuilder.CreateTable("Category", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(nullable: false)
            },
                constraints: table => { table.PrimaryKey("PK_Category", x => x.Id); });
            migrationBuilder.CreateTable("RecurringPayment", table => new
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
                    table.PrimaryKey("PK_RecurringPaymentViewModel", x => x.Id);
                    table.ForeignKey("FK_RecurringPaymentViewModel_Category_CategoryId", x => x.CategoryId, "Category", "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_RecurringPaymentViewModel_Account_ChargedAccountId", x => x.ChargedAccountId, "Account",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_RecurringPaymentViewModel_Account_TargetAccountId", x => x.TargetAccountId, "Account",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable("PaymentViewModel", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Amount = table.Column<double>(nullable: false),
                CategoryId = table.Column<int>(nullable: true),
                ChargedAccountId = table.Column<int>(nullable: false),
                ClearPaymentViewModelNow = table.Column<bool>(nullable: false),
                Date = table.Column<DateTime>(nullable: false),
                IsCleared = table.Column<bool>(nullable: false),
                IsRecurring = table.Column<bool>(nullable: false),
                IsTransfer = table.Column<bool>(nullable: false),
                Note = table.Column<string>(nullable: true),
                RecurringPaymentViewModelId = table.Column<int>(nullable: false),
                TargetAccountId = table.Column<int>(nullable: false),
                Type = table.Column<int>(nullable: false)
            },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewModel", x => x.Id);
                    table.ForeignKey("FK_PaymentViewModel_Category_CategoryId", x => x.CategoryId, "Category", "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_PaymentViewModel_Account_ChargedAccountId", x => x.ChargedAccountId, "Account", "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_PaymentViewModel_RecurringPaymentViewModel_RecurringPaymentViewModelId", x => x.RecurringPaymentViewModelId,
                        "RecurringPayment", "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_PaymentViewModel_Account_TargetAccountId", x => x.TargetAccountId, "Account", "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("PaymentViewModel");
            migrationBuilder.DropTable("RecurringPayment");
            migrationBuilder.DropTable("Category");
            migrationBuilder.DropTable("Account");
        }
    }
}