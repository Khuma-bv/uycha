using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uycha.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatesToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "PropertiesForSale",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "PropertiesForSale",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "PropertiesForSale");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "PropertiesForSale");
        }
    }
}
