namespace MoneyFox.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class RemoveIsOverdrawn : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "IsOverdrawn",
            table: "Accounts");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<bool>(
            name: "IsOverdrawn",
            table: "Accounts",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }
}
