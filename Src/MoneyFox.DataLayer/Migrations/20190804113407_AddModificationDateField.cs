using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.DataLayer.Migrations
{
    public partial class AddModificationDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "RecurringPayments",
                newName: "ModificationDate");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Categories",
                newName: "ModificationDate");

            migrationBuilder.AlterColumn<int>(
                name: "ChargedAccountId",
                table: "RecurringPayments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ModificationDate",
                table: "RecurringPayments",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "ModificationDate",
                table: "Categories",
                newName: "CreationTime");

            migrationBuilder.AlterColumn<int>(
                name: "ChargedAccountId",
                table: "RecurringPayments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayments_Accounts_ChargedAccountId",
                table: "RecurringPayments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
