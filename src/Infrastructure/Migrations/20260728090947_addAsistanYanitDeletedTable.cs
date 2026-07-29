using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAsistanYanitDeletedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "asistan_yanit",
                table: "asistan_yanit",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "kullanici_geri_bildirimi",
                table: "asistan_yanit",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
            name: "asistan_yanit_deleted",
            columns: table => new
            {
                id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                asistan_yanit = table.Column<string>(
                    type: "varchar(1000)",
                    maxLength: 1000,
                    nullable: false),

                created_at = table.Column<DateTimeOffset>(
                    type: "timestamp with time zone",
                    nullable: false,
                    defaultValueSql: "CURRENT_TIMESTAMP"),

                updated_at = table.Column<DateTimeOffset>(
                    type: "timestamp with time zone",
                    nullable: false,
                    defaultValueSql: "CURRENT_TIMESTAMP"),

                deleted_at = table.Column<DateTimeOffset>(
                    type: "timestamp with time zone",
                    nullable: false,
                    defaultValueSql: "CURRENT_TIMESTAMP"),

                cihaz_komut_id = table.Column<int>(
                    type: "integer",
                    nullable: true),

                yanit_turu = table.Column<string>(
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    defaultValue: ""),

                kullanici_geri_bildirimi = table.Column<string>(
                    type: "varchar(255)",
                    maxLength: 255,
                    nullable: true),

                session_id = table.Column<string>(
                    type: "text",
                    nullable: false),

                jsonData = table.Column<JsonDocument>(
                    type: "jsonb",
                    nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_asistan_yanit_deleted",
                    x => x.id);

                table.ForeignKey(
                    name:
                        "FK_asistan_yanit_deleted_cihaz_komutlari_cihaz_komut_id",
                    column: x => x.cihaz_komut_id,
                    principalTable: "cihaz_komutlari",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

            migrationBuilder.CreateIndex(
                name: "IX_asistan_yanit_deleted_cihaz_komut_id",
                table: "asistan_yanit_deleted",
                column: "cihaz_komut_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asistan_yanit_deleted");

            migrationBuilder.DropColumn(
                name: "kullanici_geri_bildirimi",
                table: "asistan_yanit");

            migrationBuilder.AlterColumn<string>(
                name: "asistan_yanit",
                table: "asistan_yanit",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
