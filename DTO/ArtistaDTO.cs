using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class ArtistaDTO : IValidatableObject
{
    [Required(ErrorMessage = "O nome do artista é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    public string? ImagemURL { get; set; }

    public IFormFile? ArquivoImagem { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ArquivoImagem is not null && !string.IsNullOrWhiteSpace(ImagemURL))
            yield return new ValidationResult(
                "Envie ArquivoImagem ou ImagemURL, não os dois.",
                new[] { nameof(ArquivoImagem), nameof(ImagemURL) });
    }
}
