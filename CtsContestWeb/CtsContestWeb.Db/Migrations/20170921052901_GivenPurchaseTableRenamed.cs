using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CtsContestWeb.Db.Migrations
{
    public partial class GivenPurchaseTableRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GivenPurchase_Purchases_GivenPurchaseId",
                table: "GivenPurchase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GivenPurchase",
                table: "GivenPurchase");

            migrationBuilder.RenameTable(
                name: "GivenPurchase",
                newName: "GivenPurchases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GivenPurchases",
                table: "GivenPurchases",
                column: "GivenPurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_GivenPurchases_Purchases_GivenPurchaseId",
                table: "GivenPurchases",
                column: "GivenPurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GivenPurchases_Purchases_GivenPurchaseId",
                table: "GivenPurchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GivenPurchases",
                table: "GivenPurchases");

            migrationBuilder.RenameTable(
                name: "GivenPurchases",
                newName: "GivenPurchase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GivenPurchase",
                table: "GivenPurchase",
                column: "GivenPurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_GivenPurchase_Purchases_GivenPurchaseId",
                table: "GivenPurchase",
                column: "GivenPurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
