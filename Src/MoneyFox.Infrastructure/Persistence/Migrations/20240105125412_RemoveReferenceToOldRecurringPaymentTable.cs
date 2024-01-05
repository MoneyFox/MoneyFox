using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFox.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReferenceToOldRecurringPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RecurringPayments_RecurringPaymentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RecurringPaymentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RecurringPaymentId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RecurringTransactions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RecurringTransactions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "RecurringPaymentId",
                table: "Payments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RecurringPaymentId",
                table: "Payments",
                column: "RecurringPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RecurringPayments_RecurringPaymentId",
                table: "Payments",
                column: "RecurringPaymentId",
                principalTable: "RecurringPayments",
                principalColumn: "Id");
        }
    }
}
