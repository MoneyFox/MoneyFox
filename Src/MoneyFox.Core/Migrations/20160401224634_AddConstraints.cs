using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace MoneyFox.Core.Migrations
{
    public partial class AddConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Accounts",
                nullable: false);
            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayment_Category_CategoryId",
                table: "RecurringPayments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayment_Account_ChargedAccountId",
                table: "RecurringPayments",
                column: "ChargedAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_RecurringPayment_Account_TargetAccountId",
                table: "RecurringPayments",
                column: "TargetAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_RecurringPayment_Category_CategoryId", table: "RecurringPayments");
            migrationBuilder.DropForeignKey(name: "FK_RecurringPayment_Account_ChargedAccountId", table: "RecurringPayments");
            migrationBuilder.DropForeignKey(name: "FK_RecurringPayment_Account_TargetAccountId", table: "RecurringPayments");
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Accounts",
                nullable: true);
        }
    }
}
