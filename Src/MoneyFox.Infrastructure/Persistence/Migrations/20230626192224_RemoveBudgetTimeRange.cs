using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyFox.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBudgetTimeRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetTimeRange",
                table: "Budgets");

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
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetTimeRange",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
