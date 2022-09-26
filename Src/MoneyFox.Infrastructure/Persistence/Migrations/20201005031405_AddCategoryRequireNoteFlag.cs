namespace MoneyFox.Persistence.Migrations;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

[SuppressMessage("Minor Code Smell", "S3254:Default parameter values should not be passed as arguments")]
public partial class AddCategoryRequireNoteFlag : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<bool>(name: "RequireNote",
                                         table: "Categories",
                                         type: "boolean",
                                         nullable: false,
                                         defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(name: "RequireNote",
                                    table: "Categories");
    }
}
