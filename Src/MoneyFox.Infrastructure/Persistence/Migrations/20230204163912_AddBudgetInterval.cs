using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFox.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetInterval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "IntervalNumberOfMonths",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalNumberOfMonths",
                table: "Budgets");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
