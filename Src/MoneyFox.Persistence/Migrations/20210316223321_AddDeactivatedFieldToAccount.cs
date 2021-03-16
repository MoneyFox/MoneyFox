using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.Persistence.Migrations
{
    public partial class AddDeactivatedFieldToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactivated",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactivated",
                table: "Accounts");
        }
    }
}
