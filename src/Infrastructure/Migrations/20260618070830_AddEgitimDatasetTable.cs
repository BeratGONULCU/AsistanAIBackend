using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEgitimDatasetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "egitim_dataset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tetikleyici_metin = table.Column<string>(type: "text", nullable: false),
                    type_num = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_egitim_dataset", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_egitim_dataset_tetikleyici_metin",
                table: "egitim_dataset",
                column: "tetikleyici_metin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "egitim_dataset");
        }
    }
}
