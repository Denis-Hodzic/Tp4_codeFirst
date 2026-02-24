using Microsoft.EntityFrameworkCore;
using Tp4_codeFirst.Models.EntityFramework;

namespace Tp4_codeFirst.Models.EntityFramework
{
    public partial class FilmRatingsDbContext : DbContext
    {
        public FilmRatingsDbContext() { }

        public FilmRatingsDbContext(DbContextOptions<FilmRatingsDbContext> options)
            : base(options) { }

        public virtual DbSet<Film> Films { get; set; } = null!;
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
        public virtual DbSet<Notation> Notations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ⚠️ Remplace par ta connexion (host, db, user, pwd)
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=FilmRatings;Username=postgres;Password=Dragon10");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // demandé dans l’énoncé
            modelBuilder.HasDefaultSchema("public");

            // ---- FILM ----
            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("t_e_film_flm");

                // index non unique sur flm_titre
                entity.HasIndex(e => e.Titre)
                      .HasDatabaseName("ix_flm_titre");
            });

            // ---- UTILISATEUR ----
            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.ToTable("t_e_utilisateur_utl");

                // clé unique sur mail
                entity.HasIndex(e => e.Mail)
                      .IsUnique()
                      .HasDatabaseName("uq_utl_mail");

                // valeurs par défaut
                entity.Property(e => e.Pays).HasDefaultValue("France");
                entity.Property(e => e.DateCreation).HasDefaultValueSql("CURRENT_DATE");

                // fixed length (si tu veux respecter strictement char(10)/char(5))
                entity.Property(e => e.Mobile).IsFixedLength();
                entity.Property(e => e.CodePostal).IsFixedLength();
            });

            // ---- NOTATION ----
            modelBuilder.Entity<Notation>(entity =>
            {
                entity.ToTable("t_j_notation_not");

                // check note 0..5 (base)
                entity.ToTable(t => t.HasCheckConstraint(
                    "ck_not_note_0_5",
                    "not_note >= 0 AND not_note <= 5"));

                // FKs avec nommage demandé + delete RESTRICT
                entity.HasOne(d => d.FilmNote)
                      .WithMany(p => p.NotesFilm)
                      .HasForeignKey(d => d.FilmId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_not_flm");

                entity.HasOne(d => d.UtilisateurNotant)
                      .WithMany(p => p.NotesUtilisateur)
                      .HasForeignKey(d => d.UtilisateurId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_not_utl");
            });
        }
    }
}