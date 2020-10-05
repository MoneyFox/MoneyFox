using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.Persistence.Migrations
{
    public partial class AddCategoryRequireNoteFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(name: "RequireNote",
                                             table: "Categories",
                                             type: "boolean",
                                             nullable: false,
                                             defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "RequireNote",
                                        table: "Categories");
        }
    }
}
