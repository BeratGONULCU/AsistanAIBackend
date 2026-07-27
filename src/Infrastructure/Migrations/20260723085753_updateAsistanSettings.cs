using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAsistanSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dead_word",
                table: "asistan_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "kapat");

            migrationBuilder.AddColumn<string>(
                name: "ollama_model",
                table: "asistan_settings",
                type: "text",
                nullable: true,
                defaultValue: "llama3.1:8b");

            migrationBuilder.AddColumn<string>(
                name: "wake_word",
                table: "asistan_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "asistan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dead_word",
                table: "asistan_settings");

            migrationBuilder.DropColumn(
                name: "ollama_model",
                table: "asistan_settings");

            migrationBuilder.DropColumn(
                name: "wake_word",
                table: "asistan_settings");
        }
    }
}
