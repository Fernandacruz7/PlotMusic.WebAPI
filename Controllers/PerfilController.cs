using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PerfilController : ControllerBase
{
    private readonly IPerfilRepository _perfilRepository;

    public PerfilController(IPerfilRepository perfilRepository)
    {
        _perfilRepository = perfilRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _perfilRepository.Listar());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var perfil = await _perfilRepository.BuscarPorId(id);
        return perfil is null ? NotFound() : Ok(perfil);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Cadastrar([FromBody] PerfilDTO dto)
    {
        var perfil = new Perfil { Nome = dto.Nome };
        await _perfilRepository.Cadastrar(perfil);
        return CreatedAtAction(nameof(BuscarPorId), new { id = perfil.IdPerfil }, perfil);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] PerfilDTO dto)
    {
        if (await _perfilRepository.BuscarPorId(id) is null) return NotFound();
        await _perfilRepository.Atualizar(id, new Perfil { Nome = dto.Nome });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Excluir(int id)
    {
        if (await _perfilRepository.BuscarPorId(id) is null) return NotFound();
        await _perfilRepository.Excluir(id);
        return NoContent();
    }
}
