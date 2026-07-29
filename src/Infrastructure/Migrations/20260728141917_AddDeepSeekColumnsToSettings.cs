using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeepSeekColumnsToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "deepseek_api_key",
                table: "asistan_settings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deepseek_base_url",
                table: "asistan_settings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "https://api.deepseek.com");

            migrationBuilder.AddColumn<string>(
                name: "deepseek_model",
                table: "asistan_settings",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                defaultValue: "deepseek-v4-flash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deepseek_api_key",
                table: "asistan_settings");

            migrationBuilder.DropColumn(
                name: "deepseek_base_url",
                table: "asistan_settings");

            migrationBuilder.DropColumn(
                name: "deepseek_model",
                table: "asistan_settings");
        }
    }
}
