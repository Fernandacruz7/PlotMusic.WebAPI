using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic.WebAPI.Models;

public partial class Artista
{
    [Key]
    public int IdArtista { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? ImagemURL { get; set; }

    // Navigation
    [InverseProperty("Artista")]
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    [InverseProperty("Artista")]
    public virtual ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}
