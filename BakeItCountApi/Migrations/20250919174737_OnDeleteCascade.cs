using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BakeItCountApi.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Swaps_Schedules_SourceScheduleId",
                table: "Swaps");

            migrationBuilder.DropForeignKey(
                name: "FK_Swaps_Schedules_TargetScheduleId",
                table: "Swaps");

            migrationBuilder.DropTable(
                name: "User_Achievements");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Pairs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Flavors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    UserAchievementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EarnedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => new { x.UserId, x.AchievementId });
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 1,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 2,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 3,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 4,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 5,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 6,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 7,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 8,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 9,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 10,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 11,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 12,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 13,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 14,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 15,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 16,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 17,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 18,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 19,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 20,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 21,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 22,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 23,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 24,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 25,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 26,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 27,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 28,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 29,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 30,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 31,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 32,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 33,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 34,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 35,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 36,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 37,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 38,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 39,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 40,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 41,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 42,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 43,
                column: "Votes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 44,
                column: "Votes",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementId",
                table: "UserAchievements",
                column: "AchievementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Swaps_Schedules_SourceScheduleId",
                table: "Swaps",
                column: "SourceScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Swaps_Schedules_TargetScheduleId",
                table: "Swaps",
                column: "TargetScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Swaps_Schedules_SourceScheduleId",
                table: "Swaps");

            migrationBuilder.DropForeignKey(
                name: "FK_Swaps_Schedules_TargetScheduleId",
                table: "Swaps");

            migrationBuilder.DropTable(
                name: "UserAchievements");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Flavors");

            migrationBuilder.CreateTable(
                name: "User_Achievements",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Achievements", x => new { x.UserId, x.AchievementId });
                    table.ForeignKey(
                        name: "FK_User_Achievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Achievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Achievements_AchievementId",
                table: "User_Achievements",
                column: "AchievementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Swaps_Schedules_SourceScheduleId",
                table: "Swaps",
                column: "SourceScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Swaps_Schedules_TargetScheduleId",
                table: "Swaps",
                column: "TargetScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
