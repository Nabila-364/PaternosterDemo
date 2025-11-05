using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaternosterDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddShelvesBinsAndProductionOrderParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductionOrderParts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductionOrderParts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
