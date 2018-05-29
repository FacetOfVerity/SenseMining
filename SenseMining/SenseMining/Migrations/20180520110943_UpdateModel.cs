using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SenseMining.API.Migrations
{
    public partial class UpdateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Support",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Support",
                table: "Products");

            migrationBuilder.AddColumn<long>(
                name: "Frequency",
                table: "Products",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
