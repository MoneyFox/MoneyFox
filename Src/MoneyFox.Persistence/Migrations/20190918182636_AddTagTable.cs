using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.Persistence.Migrations
{
    public partial class AddTagTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Tags",
                table => new
                {
                    Id = table.Column<int>()
                              .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Tags", x => x.Id); });

            migrationBuilder.CreateTable(
                "PaymentTag",
                table => new
                {
                    PaymentId = table.Column<int>(),
                    TagId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTag", x => new {x.PaymentId, x.TagId});
                    table.ForeignKey(
                        "FK_PaymentTag_Payments_PaymentId",
                        x => x.PaymentId,
                        "Payments",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_PaymentTag_Tags_TagId",
                        x => x.TagId,
                        "Tags",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_PaymentTag_TagId",
                "PaymentTag",
                "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PaymentTag");

            migrationBuilder.DropTable(
                "Tags");
        }
    }
}
