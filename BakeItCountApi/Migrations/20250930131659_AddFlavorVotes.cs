using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BakeItCountApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFlavorVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlavorVotes",
                columns: table => new
                {
                    FlavorVoteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FlavorId = table.Column<int>(type: "integer", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    FlavorId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlavorVotes", x => x.FlavorVoteId);
                    table.ForeignKey(
                        name: "FK_FlavorVotes_Flavors_FlavorId",
                        column: x => x.FlavorId,
                        principalTable: "Flavors",
                        principalColumn: "FlavorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlavorVotes_Flavors_FlavorId1",
                        column: x => x.FlavorId1,
                        principalTable: "Flavors",
                        principalColumn: "FlavorId");
                    table.ForeignKey(
                        name: "FK_FlavorVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlavorVotes_FlavorId",
                table: "FlavorVotes",
                column: "FlavorId");

            migrationBuilder.CreateIndex(
                name: "IX_FlavorVotes_FlavorId1",
                table: "FlavorVotes",
                column: "FlavorId1");

            migrationBuilder.CreateIndex(
                name: "IX_FlavorVotes_UserId",
                table: "FlavorVotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlavorVotes");

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

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Flavors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

           

        }
    }
}
