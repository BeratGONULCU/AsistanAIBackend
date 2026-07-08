using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRedmineTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "redmine_egitim_dataset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    redmine_tetikleyici_metin = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    sesTetikleyici_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_redmine_egitim_dataset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_redmine_egitim_dataset_ses_tetikleyicileri_sesTetikleyici_id",
                        column: x => x.sesTetikleyici_id,
                        principalTable: "ses_tetikleyicileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_redmine_egitim_dataset_redmine_tetikleyici_metin",
                table: "redmine_egitim_dataset",
                column: "redmine_tetikleyici_metin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_redmine_egitim_dataset_sesTetikleyici_id",
                table: "redmine_egitim_dataset",
                column: "sesTetikleyici_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "redmine_egitim_dataset");
        }
    }
}
