using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SenseMining.API.Migrations
{
    public partial class ChangeFpTreeConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FpTree_Products_ProductId",
                table: "FpTree");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "FpTree",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_FpTree_Products_ProductId",
                table: "FpTree",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FpTree_Products_ProductId",
                table: "FpTree");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "FpTree",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FpTree_Products_ProductId",
                table: "FpTree",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
