using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEgitimDatasetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sesTetikleyici_id",
                table: "egitim_dataset",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_egitim_dataset_sesTetikleyici_id",
                table: "egitim_dataset",
                column: "sesTetikleyici_id");

            migrationBuilder.AddForeignKey(
                name: "FK_egitim_dataset_ses_tetikleyicileri_sesTetikleyici_id",
                table: "egitim_dataset",
                column: "sesTetikleyici_id",
                principalTable: "ses_tetikleyicileri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_egitim_dataset_ses_tetikleyicileri_sesTetikleyici_id",
                table: "egitim_dataset");

            migrationBuilder.DropIndex(
                name: "IX_egitim_dataset_sesTetikleyici_id",
                table: "egitim_dataset");

            migrationBuilder.DropColumn(
                name: "sesTetikleyici_id",
                table: "egitim_dataset");
        }
    }
}
