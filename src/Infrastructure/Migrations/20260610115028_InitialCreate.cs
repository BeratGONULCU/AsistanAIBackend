using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cihaz_komutlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aksiyon_anahtari = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    calisacak_kod = table.Column<string>(type: "text", nullable: false),
                    aciklama = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cihaz_komutlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "islem_loglari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    duyulan_ses = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    cevap_metni = table.Column<string>(type: "text", nullable: true),
                    tarih_saat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    komut_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_islem_loglari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_islem_loglari_cihaz_komutlari_komut_id",
                        column: x => x.komut_id,
                        principalTable: "cihaz_komutlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ses_tetikleyicileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tetikleyici_metin = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    komut_id = table.Column<int>(type: "integer", nullable: false),
                    eklenme_turu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ses_tetikleyicileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ses_tetikleyicileri_cihaz_komutlari_komut_id",
                        column: x => x.komut_id,
                        principalTable: "cihaz_komutlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cihaz_komutlari_aksiyon_anahtari",
                table: "cihaz_komutlari",
                column: "aksiyon_anahtari",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_islem_loglari_komut_id",
                table: "islem_loglari",
                column: "komut_id");

            migrationBuilder.CreateIndex(
                name: "IX_ses_tetikleyicileri_komut_id",
                table: "ses_tetikleyicileri",
                column: "komut_id");

            migrationBuilder.CreateIndex(
                name: "IX_ses_tetikleyicileri_tetikleyici_metin",
                table: "ses_tetikleyicileri",
                column: "tetikleyici_metin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "islem_loglari");

            migrationBuilder.DropTable(
                name: "ses_tetikleyicileri");

            migrationBuilder.DropTable(
                name: "cihaz_komutlari");
        }
    }
}
