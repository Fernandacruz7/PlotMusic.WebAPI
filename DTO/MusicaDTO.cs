using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class MusicaDTO : IValidatableObject
{
    [StringLength(100)]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "A duração é obrigatória.")]
    public TimeOnly Duracao { get; set; }

    [Required(ErrorMessage = "O álbum é obrigatório.")]
    [Range(1, int.MaxValue)]
    public int IdAlbum { get; set; }

    [Required(ErrorMessage = "O artista é obrigatório.")]
    [Range(1, int.MaxValue)]
    public int IdArtista { get; set; }

    [Required(ErrorMessage = "O gênero é obrigatório.")]
    [Range(1, int.MaxValue)]
    public int IdGenero { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Duracao == TimeOnly.MinValue)
            yield return new ValidationResult(
                "A duração deve ser maior que zero.",
                new[] { nameof(Duracao) });
    }
}
