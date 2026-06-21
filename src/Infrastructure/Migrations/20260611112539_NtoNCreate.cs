using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NtoNCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ses_tetikleyicileri_cihaz_komutlari_komut_id",
                table: "ses_tetikleyicileri");

            migrationBuilder.DropIndex(
                name: "IX_ses_tetikleyicileri_komut_id",
                table: "ses_tetikleyicileri");

            migrationBuilder.DropColumn(
                name: "komut_id",
                table: "ses_tetikleyicileri");

            migrationBuilder.CreateTable(
                name: "tetikleyici_komut",
                columns: table => new
                {
                    tetikleyici_id = table.Column<int>(type: "integer", nullable: false),
                    komut_id = table.Column<int>(type: "integer", nullable: false),
                    CihazKomutuId = table.Column<int>(type: "integer", nullable: true),
                    SesTetikleyicisiId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tetikleyici_komut", x => new { x.tetikleyici_id, x.komut_id });
                    table.ForeignKey(
                        name: "FK_tetikleyici_komut_cihaz_komutlari_CihazKomutuId",
                        column: x => x.CihazKomutuId,
                        principalTable: "cihaz_komutlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tetikleyici_komut_cihaz_komutlari_komut_id",
                        column: x => x.komut_id,
                        principalTable: "cihaz_komutlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tetikleyici_komut_ses_tetikleyicileri_SesTetikleyicisiId",
                        column: x => x.SesTetikleyicisiId,
                        principalTable: "ses_tetikleyicileri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tetikleyici_komut_ses_tetikleyicileri_tetikleyici_id",
                        column: x => x.tetikleyici_id,
                        principalTable: "ses_tetikleyicileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tetikleyici_komut_CihazKomutuId",
                table: "tetikleyici_komut",
                column: "CihazKomutuId");

            migrationBuilder.CreateIndex(
                name: "IX_tetikleyici_komut_komut_id",
                table: "tetikleyici_komut",
                column: "komut_id");

            migrationBuilder.CreateIndex(
                name: "IX_tetikleyici_komut_SesTetikleyicisiId",
                table: "tetikleyici_komut",
                column: "SesTetikleyicisiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tetikleyici_komut");

            migrationBuilder.AddColumn<int>(
                name: "komut_id",
                table: "ses_tetikleyicileri",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ses_tetikleyicileri_komut_id",
                table: "ses_tetikleyicileri",
                column: "komut_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ses_tetikleyicileri_cihaz_komutlari_komut_id",
                table: "ses_tetikleyicileri",
                column: "komut_id",
                principalTable: "cihaz_komutlari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
