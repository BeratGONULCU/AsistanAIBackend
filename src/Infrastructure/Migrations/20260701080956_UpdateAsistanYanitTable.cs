using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAsistanYanitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "yanitTuruid",
                table: "asistan_yanit",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_asistan_yanit_yanitTuruid",
                table: "asistan_yanit",
                column: "yanitTuruid");

            migrationBuilder.AddForeignKey(
                name: "FK_asistan_yanit_asistan_yanit_yanitTuruid",
                table: "asistan_yanit",
                column: "yanitTuruid",
                principalTable: "asistan_yanit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asistan_yanit_asistan_yanit_yanitTuruid",
                table: "asistan_yanit");

            migrationBuilder.DropIndex(
                name: "IX_asistan_yanit_yanitTuruid",
                table: "asistan_yanit");

            migrationBuilder.DropColumn(
                name: "yanitTuruid",
                table: "asistan_yanit");
        }
    }
}
