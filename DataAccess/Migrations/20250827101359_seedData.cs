using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 1, "Sprawdzamy wiedzę ogólną", "Test wiedzy ogólnej" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "PathToFile", "QuestionType", "QuizId", "Text" },
                values: new object[] { 1, null, 1, 1, "Pytanie 1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
