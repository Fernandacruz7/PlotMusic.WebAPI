using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPerfilRepository _perfilRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository, IPerfilRepository perfilRepository)
    {
        _usuarioRepository = usuarioRepository;
        _perfilRepository = perfilRepository;
    }

    [HttpGet]
    //[Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _usuarioRepository.Listar();
        return Ok(usuarios.Select(UsuarioRespostaDTO.Converter));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        if (!PodeAcessar(id)) return Forbid();
        var usuario = await _usuarioRepository.BuscarPorId(id);
        return usuario is null ? NotFound() : Ok(UsuarioRespostaDTO.Converter(usuario));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
    {
        if (await _perfilRepository.BuscarPorId(dto.IdPerfil) is null)
            return BadRequest("Perfil informado não existe.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            IdPerfil = dto.IdPerfil,
            DataCriacao = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await _usuarioRepository.Cadastrar(usuario);
        var criado = await _usuarioRepository.BuscarPorId(usuario.IdUsuario);
        return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.IdUsuario }, UsuarioRespostaDTO.Converter(criado!));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioAtualizacaoDTO dto)
    {
        if (!PodeAcessar(id)) return Forbid();
        var atual = await _usuarioRepository.BuscarPorId(id);
        if (atual is null) return NotFound();

        if (await _perfilRepository.BuscarPorId(dto.IdPerfil) is null)
            return BadRequest("Perfil informado não existe.");

        await _usuarioRepository.Atualizar(id, new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha ?? string.Empty,
            IdPerfil = dto.IdPerfil,
            DataCriacao = atual.DataCriacao
        });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Excluir(int id)
    {
        if (!PodeAcessar(id)) return Forbid();
        if (await _usuarioRepository.BuscarPorId(id) is null) return NotFound();
        await _usuarioRepository.Excluir(id);
        return NoContent();
    }

    private bool PodeAcessar(int id) => User.IsInRole("Administrador")
        || (int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var usuarioId) && usuarioId == id);
}
