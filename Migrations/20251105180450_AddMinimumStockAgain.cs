using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaternosterDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddMinimumStockAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "Parts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Parts");
        }
    }
}
