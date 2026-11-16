using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic.WebAPI.Models;

[Index("Email", Name = "UQ_Usuario_Email", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    public int IdPerfil { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    public DateOnly DataCriacao { get; set; }

    // Navigation
    [ForeignKey("IdPerfil")]
    [InverseProperty("Usuarios")]
    public virtual Perfil Perfil { get; set; } = null!;
}
