namespace MoneyFox.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class RemoveUnusedDateFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropForeignKey(
            name: "FK_Payments_Accounts_ChargedAccountId",
            table: "Payments");

        _ = migrationBuilder.DropForeignKey(
            name: "FK_RecurringPayments_Accounts_ChargedAccountId",
            table: "RecurringPayments");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "RecurringPayments");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "RecurringPayments");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "Payments");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "Payments");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "Categories");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "Categories");

        _ = migrationBuilder.DropColumn(
            name: "CreationTime",
            table: "Accounts");

        _ = migrationBuilder.DropColumn(
            name: "ModificationDate",
            table: "Accounts");

        _ = migrationBuilder.AlterColumn<int>(
            name: "ChargedAccountId",
            table: "RecurringPayments",
            type: "INTEGER",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        _ = migrationBuilder.AlterColumn<int>(
            name: "ChargedAccountId",
            table: "Payments",
            type: "INTEGER",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        _ = migrationBuilder.AddForeignKey(
            name: "FK_Payments_Accounts_ChargedAccountId",
            table: "Payments",
            column: "ChargedAccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        _ = migrationBuilder.AddForeignKey(
            name: "FK_RecurringPayments_Accounts_ChargedAccountId",
            table: "RecurringPayments",
            column: "ChargedAccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropForeignKey(
            name: "FK_Payments_Accounts_ChargedAccountId",
            table: "Payments");

        _ = migrationBuilder.DropForeignKey(
            name: "FK_RecurringPayments_Accounts_ChargedAccountId",
            table: "RecurringPayments");

        _ = migrationBuilder.AlterColumn<int>(
            name: "ChargedAccountId",
            table: "RecurringPayments",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "INTEGER",
            oldNullable: true);

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "RecurringPayments",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "RecurringPayments",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AlterColumn<int>(
            name: "ChargedAccountId",
            table: "Payments",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "INTEGER",
            oldNullable: true);

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "Payments",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "Payments",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "Categories",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "Categories",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "CreationTime",
            table: "Accounts",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddColumn<DateTime>(
            name: "ModificationDate",
            table: "Accounts",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        _ = migrationBuilder.AddForeignKey(
            name: "FK_Payments_Accounts_ChargedAccountId",
            table: "Payments",
            column: "ChargedAccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        _ = migrationBuilder.AddForeignKey(
            name: "FK_RecurringPayments_Accounts_ChargedAccountId",
            table: "RecurringPayments",
            column: "ChargedAccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
