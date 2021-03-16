using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.Persistence.Migrations
{
    public partial class AddDisabledFieldToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Accounts");
        }
    }
}
