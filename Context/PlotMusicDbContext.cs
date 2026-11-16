using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Context;

public partial class PlotMusicDbContext : DbContext
{
    public PlotMusicDbContext()
    {
    }

    public PlotMusicDbContext(DbContextOptions<PlotMusicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Perfil> Perfil { get; set; }
    public virtual DbSet<Usuario> Usuario { get; set; }
    public virtual DbSet<Artista> Artista { get; set; }
    public virtual DbSet<Album> Album { get; set; }
    public virtual DbSet<Genero> Genero { get; set; }
    public virtual DbSet<Musica> Musica { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Perfil
        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100).IsUnicode(false);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.DataCriacao).HasDefaultValueSql("CONVERT(date, GETDATE())");

            entity.HasOne(e => e.Perfil)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(e => e.IdPerfil)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_UsuarioPerfil");
        });

        // Artista
        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasKey(e => e.IdArtista);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.ImagemURL).HasMaxLength(255).IsUnicode(false);
        });

        // Album
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.IdAlbum);
            entity.Property(e => e.Titulo).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.ImagemURL).HasMaxLength(255).IsUnicode(false);

            entity.HasOne(e => e.Artista)
                .WithMany(a => a.Albums)
                .HasForeignKey(e => e.IdArtista)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_AlbumArtista");
        });

        // Genero
        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero);
            entity.Property(e => e.Nome).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Descricao).IsUnicode(false);
        });

        // Musica
        modelBuilder.Entity<Musica>(entity =>
        {
            entity.HasKey(e => e.IdMusica);
            entity.Property(e => e.Titulo).HasMaxLength(100).IsUnicode(false);

            entity.HasOne(e => e.Album)
                .WithMany(a => a.Musicas)
                .HasForeignKey(e => e.IdAlbum)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_MusicaAlbum");

            entity.HasOne(e => e.Artista)
                .WithMany(a => a.Musicas)
                .HasForeignKey(e => e.IdArtista)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fk_MusicaArtista");

            entity.HasOne(e => e.Genero)
                .WithMany(g => g.Musicas)
                .HasForeignKey(e => e.IdGenero)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fk_MusicaGenero");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
