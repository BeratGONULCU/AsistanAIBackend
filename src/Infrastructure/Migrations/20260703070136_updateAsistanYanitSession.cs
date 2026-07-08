using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAsistanYanitSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Önce eski default değeri kaldırıyoruz
            migrationBuilder.Sql("ALTER TABLE asistan_yanit ALTER COLUMN session_id DROP DEFAULT;");

            // Boş string ('') veya boşluk içeren satırları '0' string değerine çekiyoruz
            migrationBuilder.Sql("UPDATE asistan_yanit SET session_id = '0' WHERE session_id = '' OR session_id IS NULL OR TRIM(session_id) = '';");

            // 2. Şimdi kolon tipini integer'a dönüştürüyoruz
            migrationBuilder.Sql("ALTER TABLE asistan_yanit ALTER COLUMN session_id TYPE integer USING session_id::integer;");

            // 3. (Opsiyonel ama garanti olması için) Yeni integer kolonu için default değer atıyoruz (Örn: 0)
            migrationBuilder.Sql("ALTER TABLE asistan_yanit ALTER COLUMN session_id SET DEFAULT 0;");

            migrationBuilder.AlterColumn<int>(
                name: "session_id",
                table: "asistan_yanit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "session_id",
                table: "asistan_yanit",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
