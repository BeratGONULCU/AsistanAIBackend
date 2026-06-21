using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cihaz_komutlari_domain_target_operation",
                table: "cihaz_komutlari");

            migrationBuilder.AlterColumn<string>(
                name: "target",
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
                name: "target",
                table: "cihaz_komutlari",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cihaz_komutlari_domain_target_operation",
                table: "cihaz_komutlari",
                columns: new[] { "domain", "target", "operation" },
                unique: true);
        }
    }
}
