using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.DataAccess.Migrations
{
    public partial class AddCategoryGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_GroupId",
                table: "Categories",
                column: "GroupId");
            
            migrationBuilder.Sql(
                @"PRAGMA foreign_keys = 0;
 
                  CREATE TABLE Categories_temp AS SELECT *
                                                FROM Categories;
                  
                  DROP TABLE Categories;
                  
                  CREATE TABLE Categories ( 

                    Id                  INTEGER NOT NULL 
                                             CONSTRAINT PK_Categories PRIMARY KEY AUTOINCREMENT, 
                    Name                TEXT NOT NULL, 
                    Note                TEXT,
                    GroupId     INTEGER,
                    FOREIGN KEY (GroupId) REFERENCES CategoryGroups (Id) ON DELETE SET NULL 
                  );
                  
                  INSERT INTO Categories 
                  (
                      Id,
                      Name,
                      Note
                  )
                  SELECT Id,
                      Name,
                      Note
                  FROM Categories_temp;
                  
                  DROP TABLE Categories_temp;
                  
                  PRAGMA foreign_keys = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Groups_GroupId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_TargetAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringPayments_Accounts_TargetAccountId",
                table: "RecurringPayments");

            migrationBuilder.DropTable(
                name: "CategoryGroups");

            migrationBuilder.DropIndex(
                name: "IX_Categories_GroupId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "RecurringPayments",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Payments",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Accounts",
                newName: "Note");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_TargetAccountId",
                table: "Payments",
                column: "TargetAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayments_Accounts_TargetAccountId",
                table: "RecurringPayments",
                column: "TargetAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
