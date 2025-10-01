using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaternosterDemo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cabinets_CabinetId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Parts_PartId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Transactions",
                newName: "QuantityChanged");

            migrationBuilder.AlterColumn<int>(
                name: "PartId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CabinetId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InventoryId",
                table: "Transactions",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cabinets_CabinetId",
                table: "Transactions",
                column: "CabinetId",
                principalTable: "Cabinets",
                principalColumn: "CabinetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Inventories_InventoryId",
                table: "Transactions",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Parts_PartId",
                table: "Transactions",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "PartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cabinets_CabinetId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Inventories_InventoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Parts_PartId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InventoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "QuantityChanged",
                table: "Transactions",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "PartId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CabinetId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cabinets_CabinetId",
                table: "Transactions",
                column: "CabinetId",
                principalTable: "Cabinets",
                principalColumn: "CabinetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Parts_PartId",
                table: "Transactions",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "PartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
