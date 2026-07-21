using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GeminiAsistanBackend.Application.Interfaces;

namespace GeminiAsistanBackend.Infrastructure;

public class AppDbContext : DbContext , IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CihazKomutu> CihazKomutlari => Set<CihazKomutu>();
    public DbSet<SesTetikleyicisi> SesTetikleyicileri => Set<SesTetikleyicisi>();
    public DbSet<IslemLog> IslemLoglari => Set<IslemLog>();
    public DbSet<TetikleyiciKomut> TetikleyiciKomutlar => Set<TetikleyiciKomut>();
    public DbSet<EgitimDataset> EgitimDataset => Set<EgitimDataset>();
    public DbSet<AsistanYanit> AsistanYanit => Set<AsistanYanit>();

    public DbSet<RedmineEgitimDataset> RedmineEgitimDataset => Set<RedmineEgitimDataset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table names
        modelBuilder.Entity<CihazKomutu>().ToTable("cihaz_komutlari");
        modelBuilder.Entity<SesTetikleyicisi>().ToTable("ses_tetikleyicileri");
        modelBuilder.Entity<IslemLog>().ToTable("islem_loglari");
        modelBuilder.Entity<TetikleyiciKomut>().ToTable("tetikleyici_komut");
        modelBuilder.Entity<EgitimDataset>().ToTable("egitim_dataset");
        modelBuilder.Entity<RedmineEgitimDataset>().ToTable("redmine_egitim_dataset");
        modelBuilder.Entity<AsistanYanit>().ToTable("asistan_yanit");

        // CihazKomutu
        modelBuilder.Entity<CihazKomutu>(eb =>
        {
            eb.HasKey(e => e.Id);
            /*
            eb.Property(e => e.AksiyonAnahtari)
                .IsRequired()
                .HasColumnName("aksiyon_anahtari")
                .HasMaxLength(200);
            eb.HasIndex(e => e.AksiyonAnahtari).IsUnique();
            */
            eb.Property(e => e.type)
                .HasColumnName("type")
                .IsRequired(true);
            eb.Property(e => e.domain)
                .HasColumnName("domain")
                .IsRequired(false);
            eb.Property(e => e.target)
                .HasColumnName("target")
                .IsRequired(false);
            eb.Property(e => e.operation)
                .HasColumnName("operation")
                .IsRequired(true);
            eb.Property(e => e.CalisacakKod)
                .IsRequired(false)
                .HasColumnName("calisacak_kod");
            eb.Property(e => e.Aciklama)
                .HasColumnName("aciklama");

            /*eb.HasIndex(e => new { e.domain, e.operation })
                  .IsUnique();*/
        });

        // SesTetikleyicisi
        modelBuilder.Entity<SesTetikleyicisi>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.TetikleyiciMetin)
                .IsRequired()
                .HasColumnName("tetikleyici_metin")
                .HasMaxLength(500);
            eb.HasIndex(e => e.TetikleyiciMetin).IsUnique();
            //eb.Property(e => e.KomutId).HasColumnName("komut_id");

            // Store enum as string
            eb.Property(e => e.EklenmeTuru)
                .HasConversion(new EnumToStringConverter<EklenmeTuru>())
                .HasColumnName("eklenme_turu")
                .HasMaxLength(50);
            eb.Property(e => e.llm_confidence_score)
                .HasColumnName("llm_confidence_score")
                .IsRequired(false);


            eb.Property(e => e.created_at)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            eb.Property(e => e.updated_at)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            /*
            // Relationship: CihazKomutu 1 - * SesTetikleyicileri, cascade delete
            eb.HasOne(e => e.Komut)
                .WithMany(c => c.TetikleyiciKomuts)
                .HasForeignKey(e => e.KomutId)
                .OnDelete(DeleteBehavior.Cascade);
            */
        });

        modelBuilder.Entity<TetikleyiciKomut>(eb =>
        {
            eb.ToTable("tetikleyici_komut");

            eb.HasKey(e => new { e.TetikleyiciId,e.KomutId });
            eb.Property(e => e.TetikleyiciId).HasColumnName("tetikleyici_id");
            eb.Property(e => e.KomutId).HasColumnName("komut_id");

            eb.HasOne(e => e.Komut)
                .WithMany(e => e.TetikleyiciKomutlari)
                .HasForeignKey(e => e.KomutId)
                .OnDelete(DeleteBehavior.Cascade);

            eb.HasOne(e => e.Tetikleyici)
                .WithMany(e => e.TetikleyiciKomutlari)
                .HasForeignKey(e => e.TetikleyiciId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AsistanYanit
        modelBuilder.Entity<AsistanYanit>(eb =>
        {
            eb.HasKey(e => e.id);

            eb.Property(e => e.asistan_yanit)
                .IsRequired(true)
                .HasColumnName("asistan_yanit")
                .HasMaxLength(1000);

            // ENUM CONFIGURATION
            eb.Property(e => e.yanitTuru)
                .IsRequired(true)
                .HasColumnName("yanit_turu") // DB'deki kolon adı 'yanit_turu' olacak
                .HasMaxLength(50)
                .HasConversion<string>(); // C#'taki enum'ı DB'ye string/varchar olarak kaydeder!

            eb.Property(e => e.created_at)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            eb.Property(e => e.updated_at)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            eb.Property(e => e.SessionId)
                .HasColumnName("session_id")
                .IsRequired(true);

            eb.Property(e => e.feedback)
                .HasColumnName("feedback")
                .HasColumnType("varchar(255)")
                .IsRequired(false);

            eb.Property(e => e.cihaz_komut_id)
                .HasColumnName("cihaz_komut_id")
                .IsRequired(false);

            eb.Property(e => e.JsonData)
                .HasColumnName("JsonData")
                .HasColumnType("jsonb")
                .IsRequired(false);

            eb.HasOne(e => e.cihazkomutu)
                .WithMany(c => c.AsistanYanitlar)
                .HasForeignKey(e => e.cihaz_komut_id)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // IslemLog
        modelBuilder.Entity<IslemLog>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.DuyulanSes)
                .IsRequired()
                .HasColumnName("duyulan_ses")
                .HasMaxLength(1000);
            // Store enum as string
            eb.Property(e => e.Durum)
                .HasConversion(new EnumToStringConverter<IslemDurum>())
                .IsRequired()
                .HasColumnName("durum")
                .HasMaxLength(50);
            eb.Property(e => e.CevapMetni)
                .HasColumnName("cevap_metni");
            // Default timestamp (works on SQLite/Postgres); for SQL Server consider GETDATE()
            eb.Property(e => e.TarihSaat)
                .HasColumnName("tarih_saat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            eb.Property(e => e.KomutId)
                .HasColumnName("komut_id")
                .IsRequired(false);
            eb.Property(e => e.raw_ai_json)
                .HasColumnName("raw_ai_json")
                .IsRequired(false);

            // Relationship: CihazKomutu 1 - * IslemLoglari, set null on delete
            eb.HasOne(e => e.Komut)
                .WithMany(c => c.IslemLoglari)
                .HasForeignKey(e => e.KomutId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<EgitimDataset>(eb => {
            eb.ToTable("egitim_dataset");

            eb.HasKey(e => e.Id);
            eb.Property(e => e.tetikleyici_metin)
                .HasColumnName("tetikleyici_metin")
                .IsRequired(true);

            eb.HasIndex(e => e.tetikleyici_metin).IsUnique();


            eb.Property(e => e.type_num)
                .HasColumnName("type_num")
                .IsRequired(true);

            eb.HasOne(e => e.SesTetikleyicisi)
                .WithMany(c => c.EgitimDatasetleri)
                .HasForeignKey(e => e.sesTetikleyici_id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RedmineEgitimDataset>(eb =>
        {
            eb.ToTable("redmine_egitim_dataset");

            eb.HasKey(e => e.Id);
            eb.Property(e => e.redmine_tetikleyici_metin)
                .HasColumnName("redmine_tetikleyici_metin")
                .IsRequired(true);

            eb.HasIndex(e => e.redmine_tetikleyici_metin).IsUnique();

            eb.Property(eb => eb.action)
                .HasColumnName("action")
                .IsRequired(true);

            eb.HasOne(e => e.sesTetikleyicisi)
                .WithMany(c => c.RedmineEgitimDatasets)
                .HasForeignKey(e => e.sesTetikleyici_id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
