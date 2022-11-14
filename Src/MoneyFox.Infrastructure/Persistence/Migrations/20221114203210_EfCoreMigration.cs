using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFox.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EfCoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_ChargedAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments");

            migrationBuilder.AlterColumn<int>(
                name: "ChargedAccountId",
                table: "RecurringPayments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChargedAccountId",
                table: "Payments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_ChargedAccountId",
                table: "Payments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_ChargedAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments");

            migrationBuilder.AlterColumn<int>(
                name: "ChargedAccountId",
                table: "RecurringPayments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ChargedAccountId",
                table: "Payments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_ChargedAccountId",
                table: "Payments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
