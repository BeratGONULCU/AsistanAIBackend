using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnProperty2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "calisacak_kod",
                table: "cihaz_komutlari",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "calisacak_kod",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
