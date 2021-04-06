using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DecorAndHandicraftMerchant.Data.Migrations
{
    public partial class addedFieldsToCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Carts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "Carts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Carts");
        }
    }
}
