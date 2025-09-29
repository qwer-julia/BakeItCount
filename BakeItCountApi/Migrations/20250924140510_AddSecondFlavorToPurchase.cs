using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeItCountApi.Migrations
{
    public partial class AddSecondFlavorToPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Flavors_FlavorId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "FlavorId",
                table: "Purchases",
                newName: "Flavor2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_FlavorId",
                table: "Purchases",
                newName: "IX_Purchases_Flavor2Id");

            // ✅ corrigido: nullable, sem defaultValue
            migrationBuilder.AddColumn<int>(
                name: "Flavor1Id",
                table: "Purchases",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Flavor1Id",
                table: "Purchases",
                column: "Flavor1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Flavors_Flavor1Id",
                table: "Purchases",
                column: "Flavor1Id",
                principalTable: "Flavors",
                principalColumn: "FlavorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Flavors_Flavor2Id",
                table: "Purchases",
                column: "Flavor2Id",
                principalTable: "Flavors",
                principalColumn: "FlavorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Flavors_Flavor1Id",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Flavors_Flavor2Id",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_Flavor1Id",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Flavor1Id",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "Flavor2Id",
                table: "Purchases",
                newName: "FlavorId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_Flavor2Id",
                table: "Purchases",
                newName: "IX_Purchases_FlavorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Flavors_FlavorId",
                table: "Purchases",
                column: "FlavorId",
                principalTable: "Flavors",
                principalColumn: "FlavorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
