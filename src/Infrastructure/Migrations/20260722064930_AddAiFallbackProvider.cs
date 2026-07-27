using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAiFallbackProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ai_fallback_provider",
                table: "asistan_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "ollama");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ai_fallback_provider",
                table: "asistan_settings");
        }
    }
}
