namespace MoneyFox.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Minor Code Smell", "S3254:Default parameter values should not be passed as arguments")]
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
