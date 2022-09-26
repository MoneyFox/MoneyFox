namespace MoneyFox.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddIsLastDayOfMonthToRecurringPayment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<bool>(
            name: "IsLastDayOfMonth",
            table: "RecurringPayments",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "IsLastDayOfMonth",
            table: "RecurringPayments");
    }
}
