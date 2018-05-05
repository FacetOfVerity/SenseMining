using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SenseMining.API.Migrations
{
    public partial class ChangeFpTreeConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FpTree_Products_Id",
                table: "FpTree");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "FpTree",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FpTree_ProductId",
                table: "FpTree",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FpTree_Products_ProductId",
                table: "FpTree",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FpTree_Products_ProductId",
                table: "FpTree");

            migrationBuilder.DropIndex(
                name: "IX_FpTree_ProductId",
                table: "FpTree");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "FpTree");

            migrationBuilder.AddForeignKey(
                name: "FK_FpTree_Products_Id",
                table: "FpTree",
                column: "Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
