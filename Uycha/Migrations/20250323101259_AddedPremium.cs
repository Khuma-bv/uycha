using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uycha.Migrations
{
    /// <inheritdoc />
    public partial class AddedPremium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "PropertiesForSale",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "PropertiesForSale");
        }
    }
}
