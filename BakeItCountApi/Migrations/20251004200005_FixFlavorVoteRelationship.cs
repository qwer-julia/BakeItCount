using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeItCountApi.Migrations
{
    /// <inheritdoc />
    public partial class FixFlavorVoteRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FlavorVotes_FlavorId1",
                table: "FlavorVotes",
                column: "FlavorId1");

            migrationBuilder.Sql(@"
    DO $$
    BEGIN
        IF EXISTS (
            SELECT 1 
            FROM information_schema.columns 
            WHERE table_name='FlavorVotes' AND column_name='FlavorId1'
        ) THEN
            ALTER TABLE ""FlavorVotes"" DROP COLUMN ""FlavorId1"";
        END IF;
    END$$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
    name: "FlavorId1",
    table: "FlavorVotes",
    type: "integer",
    nullable: true);
        }
    }
}
