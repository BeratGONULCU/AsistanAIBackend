using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiAsistanBackend.Infrastructure.Migrations
{
    public partial class AddAsistanYanitDeleteTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE OR REPLACE FUNCTION public.archive_deleted_asistan_yanit()
                RETURNS TRIGGER
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    INSERT INTO public.asistan_yanit_deleted
                    (
                        asistan_yanit,
                        created_at,
                        updated_at,
                        deleted_at,
                        cihaz_komut_id,
                        yanit_turu,
                        kullanici_geri_bildirimi,
                        session_id,
                        "jsonData"
                    )
                    VALUES
                    (
                        OLD.asistan_yanit,
                        OLD.created_at,
                        OLD.updated_at,
                        CURRENT_TIMESTAMP,
                        OLD.cihaz_komut_id,
                        OLD.yanit_turu,
                        COALESCE(
                            OLD.kullanici_geri_bildirimi,
                            OLD.feedback
                        ),
                        OLD.session_id,
                        OLD."JsonData"
                    );

                    RETURN OLD;
                END;
                $$;

                DROP TRIGGER IF EXISTS
                    trg_archive_deleted_asistan_yanit
                ON public.asistan_yanit;

                CREATE TRIGGER trg_archive_deleted_asistan_yanit
                AFTER DELETE
                ON public.asistan_yanit
                FOR EACH ROW
                EXECUTE FUNCTION public.archive_deleted_asistan_yanit();
                """
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP TRIGGER IF EXISTS
                    trg_archive_deleted_asistan_yanit
                ON public.asistan_yanit;

                DROP FUNCTION IF EXISTS
                    public.archive_deleted_asistan_yanit();
                """
            );
        }
    }
}