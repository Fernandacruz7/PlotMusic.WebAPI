using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class GeneroDTO
{
    [StringLength(100)]
    public string? Nome { get; set; }

    public string? Descricao { get; set; }
}
