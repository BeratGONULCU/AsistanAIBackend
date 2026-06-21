using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cihaz_komutlari_aksiyon_anahtari",
                table: "cihaz_komutlari");

            migrationBuilder.DropColumn(
                name: "aksiyon_anahtari",
                table: "cihaz_komutlari");

            migrationBuilder.AddColumn<double>(
                name: "ai_confidence_score",
                table: "ses_tetikleyicileri",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "raw_ai_json",
                table: "islem_loglari",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "cihaz_komutlari",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "domain",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "operation",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "target",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_cihaz_komutlari_domain_target_operation",
                table: "cihaz_komutlari",
                columns: new[] { "domain", "target", "operation" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cihaz_komutlari_domain_target_operation",
                table: "cihaz_komutlari");

            migrationBuilder.DropColumn(
                name: "ai_confidence_score",
                table: "ses_tetikleyicileri");

            migrationBuilder.DropColumn(
                name: "raw_ai_json",
                table: "islem_loglari");

            migrationBuilder.DropColumn(
                name: "domain",
                table: "cihaz_komutlari");

            migrationBuilder.DropColumn(
                name: "operation",
                table: "cihaz_komutlari");

            migrationBuilder.DropColumn(
                name: "target",
                table: "cihaz_komutlari");

            migrationBuilder.DropColumn(
                name: "type",
                table: "cihaz_komutlari");

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aksiyon_anahtari",
                table: "cihaz_komutlari",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_cihaz_komutlari_aksiyon_anahtari",
                table: "cihaz_komutlari",
                column: "aksiyon_anahtari",
                unique: true);
        }
    }
}
