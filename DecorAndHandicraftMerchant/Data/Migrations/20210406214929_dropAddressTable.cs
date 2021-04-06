using Microsoft.EntityFrameworkCore.Migrations;

namespace DecorAndHandicraftMerchant.Data.Migrations
{
    public partial class dropAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AddressId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Profiles",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Profiles",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Profiles",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Profiles",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Profiles",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Profiles");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
