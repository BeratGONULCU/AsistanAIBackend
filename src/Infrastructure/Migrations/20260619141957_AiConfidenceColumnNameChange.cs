using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AiConfidenceColumnNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ai_confidence_score",
                table: "ses_tetikleyicileri",
                newName: "llm_confidence_score");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "llm_confidence_score",
                table: "ses_tetikleyicileri",
                newName: "ai_confidence_score");
        }
    }
}
