using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTetikleyiciKomutRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tetikleyici_komut_cihaz_komutlari_CihazKomutuId",
                table: "tetikleyici_komut");

            migrationBuilder.DropForeignKey(
                name: "FK_tetikleyici_komut_ses_tetikleyicileri_SesTetikleyicisiId",
                table: "tetikleyici_komut");

            migrationBuilder.DropIndex(
                name: "IX_tetikleyici_komut_CihazKomutuId",
                table: "tetikleyici_komut");

            migrationBuilder.DropIndex(
                name: "IX_tetikleyici_komut_SesTetikleyicisiId",
                table: "tetikleyici_komut");

            migrationBuilder.DropColumn(
                name: "CihazKomutuId",
                table: "tetikleyici_komut");

            migrationBuilder.DropColumn(
                name: "SesTetikleyicisiId",
                table: "tetikleyici_komut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CihazKomutuId",
                table: "tetikleyici_komut",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SesTetikleyicisiId",
                table: "tetikleyici_komut",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tetikleyici_komut_CihazKomutuId",
                table: "tetikleyici_komut",
                column: "CihazKomutuId");

            migrationBuilder.CreateIndex(
                name: "IX_tetikleyici_komut_SesTetikleyicisiId",
                table: "tetikleyici_komut",
                column: "SesTetikleyicisiId");

            migrationBuilder.AddForeignKey(
                name: "FK_tetikleyici_komut_cihaz_komutlari_CihazKomutuId",
                table: "tetikleyici_komut",
                column: "CihazKomutuId",
                principalTable: "cihaz_komutlari",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tetikleyici_komut_ses_tetikleyicileri_SesTetikleyicisiId",
                table: "tetikleyici_komut",
                column: "SesTetikleyicisiId",
                principalTable: "ses_tetikleyicileri",
                principalColumn: "Id");
        }
    }
}
