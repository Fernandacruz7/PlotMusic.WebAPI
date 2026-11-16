using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class PerfilDTO
{
    [Required(ErrorMessage = "O nome do perfil é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;
}
