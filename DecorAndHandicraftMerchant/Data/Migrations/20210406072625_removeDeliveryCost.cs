using Microsoft.EntityFrameworkCore.Migrations;

namespace DecorAndHandicraftMerchant.Data.Migrations
{
    public partial class removeDeliveryCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCost",
                table: "Carts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryCost",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
