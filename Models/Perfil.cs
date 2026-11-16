using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic.WebAPI.Models;

public partial class Perfil
{
    [Key]
    public int IdPerfil { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    // Navigation
    [InverseProperty("Perfil")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
