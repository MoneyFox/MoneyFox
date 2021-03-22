using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Persistence.Migrations
{
    [SuppressMessage("Minor Code Smell", "S3254:Default parameter values should not be passed as arguments", Justification = "Generated Code")]
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
