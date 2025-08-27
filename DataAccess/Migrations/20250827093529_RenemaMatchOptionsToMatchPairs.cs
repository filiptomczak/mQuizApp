using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenemaMatchOptionsToMatchPairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchOptions_Questions_QuestionId",
                table: "MatchOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchOptions",
                table: "MatchOptions");

            migrationBuilder.RenameTable(
                name: "MatchOptions",
                newName: "MatchPairs");

            migrationBuilder.RenameIndex(
                name: "IX_MatchOptions_QuestionId",
                table: "MatchPairs",
                newName: "IX_MatchPairs_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchPairs",
                table: "MatchPairs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPairs_Questions_QuestionId",
                table: "MatchPairs",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchPairs_Questions_QuestionId",
                table: "MatchPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchPairs",
                table: "MatchPairs");

            migrationBuilder.RenameTable(
                name: "MatchPairs",
                newName: "MatchOptions");

            migrationBuilder.RenameIndex(
                name: "IX_MatchPairs_QuestionId",
                table: "MatchOptions",
                newName: "IX_MatchOptions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchOptions",
                table: "MatchOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchOptions_Questions_QuestionId",
                table: "MatchOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
