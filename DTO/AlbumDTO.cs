using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class AlbumDTO : IValidatableObject
{
    [StringLength(100)]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "O ano de lançamento é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O ano de lançamento deve ser positivo.")]
    public int AnoLancamento { get; set; }

    [Required(ErrorMessage = "O artista é obrigatório.")]
    [Range(1, int.MaxValue)]
    public int IdArtista { get; set; }

    public string? ImagemURL { get; set; }

    public IFormFile? ArquivoImagem { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ArquivoImagem is not null && !string.IsNullOrWhiteSpace(ImagemURL))
            yield return new ValidationResult(
                "Envie ArquivoImagem ou ImagemURL, não os dois.",
                new[] { nameof(ArquivoImagem), nameof(ImagemURL) });

        if (AnoLancamento > DateTime.UtcNow.Year)
            yield return new ValidationResult(
                "O ano de lançamento não pode ser no futuro.",
                new[] { nameof(AnoLancamento) });
    }
}
