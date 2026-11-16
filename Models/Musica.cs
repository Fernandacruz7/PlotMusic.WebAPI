using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic.WebAPI.Models;

public partial class Musica
{
    [Key]
    public int IdMusica { get; set; }

    public int IdAlbum { get; set; }

    public int IdArtista { get; set; }

    public int IdGenero { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    public TimeOnly Duracao { get; set; }

    // Navigation
    [ForeignKey("IdAlbum")]
    [InverseProperty("Musicas")]
    public virtual Album Album { get; set; } = null!;

    [ForeignKey("IdArtista")]
    [InverseProperty("Musicas")]
    public virtual Artista Artista { get; set; } = null!;

    [ForeignKey("IdGenero")]
    [InverseProperty("Musicas")]
    public virtual Genero Genero { get; set; } = null!;
}
