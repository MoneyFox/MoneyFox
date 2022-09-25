namespace MoneyFox.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddTimeRangeColumn : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<int>(
            name: "Id",
            table: "Budgets",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "INTEGER")
            .OldAnnotation("Sqlite:Autoincrement", true);

        _ = migrationBuilder.AddColumn<int>(
            name: "BudgetTimeRange",
            table: "Budgets",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "BudgetTimeRange",
            table: "Budgets");

        _ = migrationBuilder.AlterColumn<int>(
            name: "Id",
            table: "Budgets",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "INTEGER")
            .Annotation("Sqlite:Autoincrement", true);
    }
}
