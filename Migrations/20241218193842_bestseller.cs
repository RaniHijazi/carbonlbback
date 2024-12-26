using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreProject.Migrations
{
    public partial class bestseller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Bestseller",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NewArrival",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SpecialOffer",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bestseller",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "NewArrival",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SpecialOffer",
                table: "Items");
        }
    }
}
