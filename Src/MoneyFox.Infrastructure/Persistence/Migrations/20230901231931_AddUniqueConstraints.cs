using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFox.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactions_RecurringTransactionId",
                table: "RecurringTransactions",
                column: "RecurringTransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecurringTransactions_RecurringTransactionId",
                table: "RecurringTransactions");
        }
    }
}
