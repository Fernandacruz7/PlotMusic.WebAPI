using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic.WebAPI.Models;

public partial class Genero
{
    [Key]
    public int IdGenero { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Nome { get; set; }

    [Unicode(false)]
    public string? Descricao { get; set; }

    // Navigation
    [InverseProperty("Genero")]
    public virtual ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}
