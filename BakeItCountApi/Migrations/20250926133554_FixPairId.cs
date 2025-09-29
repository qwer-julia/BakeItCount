using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeItCountApi.Migrations
{
    /// <inheritdoc />
    public partial class FixPairId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PairId1",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId1",
                table: "Purchases",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PairId1",
                table: "Schedules",
                column: "PairId1");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ScheduleId1",
                table: "Purchases",
                column: "ScheduleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Schedules_ScheduleId1",
                table: "Purchases",
                column: "ScheduleId1",
                principalTable: "Schedules",
                principalColumn: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Pairs_PairId1",
                table: "Schedules",
                column: "PairId1",
                principalTable: "Pairs",
                principalColumn: "PairId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Schedules_ScheduleId1",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Pairs_PairId1",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_PairId1",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ScheduleId1",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PairId1",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ScheduleId1",
                table: "Purchases");
        }
    }
}
