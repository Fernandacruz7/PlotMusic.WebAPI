using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class UsuarioDTO : IValidatableObject
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O perfil é obrigatório.")]
    [Range(1, int.MaxValue)]
    public int IdPerfil { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break; // Adicione validações customizadas aqui se necessário
    }
}
