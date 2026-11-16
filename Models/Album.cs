using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic.WebAPI.Models;

public partial class Album
{
    [Key]
    public int IdAlbum { get; set; }

    public int IdArtista { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    public int AnoLancamento { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ImagemURL { get; set; }

    // Navigation
    [ForeignKey("IdArtista")]
    [InverseProperty("Albums")]
    public virtual Artista Artista { get; set; } = null!;

    [InverseProperty("Album")]
    public virtual ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}
