using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MusicaController : ControllerBase
{
    private readonly IMusicaRepository _musicaRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistaRepository _artistaRepository;
    private readonly IGeneroRepository _generoRepository;

    public MusicaController(
        IMusicaRepository musicaRepository,
        IAlbumRepository albumRepository,
        IArtistaRepository artistaRepository,
        IGeneroRepository generoRepository)
    {
        _musicaRepository = musicaRepository;
        _albumRepository = albumRepository;
        _artistaRepository = artistaRepository;
        _generoRepository = generoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Lista(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        [FromQuery] string? busca = null)
    {
        if (pagina < 1 || tamanhoPagina is < 1 or > 100)
            return BadRequest("Página deve ser positiva e tamanhoPagina deve estar entre 1 e 100.");

        var (items, total) = await _musicaRepository.ListarPaginado(pagina, tamanhoPagina, busca);
        return Ok(new { items, total, pagina, tamanhoPagina });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var musica = await _musicaRepository.BuscarPorId(id);
        return musica is null ? NotFound() : Ok(musica);
    }

    [HttpGet("album/{idAlbum:int}")]
    public async Task<IActionResult> ListarPorAlbum(int idAlbum)
    {
        if (await _albumRepository.BuscarPorId(idAlbum) is null)
            return NotFound("Álbum não encontrado.");
        return Ok(await _musicaRepository.ListarPorAlbum(idAlbum));
    }

    [HttpGet("artista/{idArtista:int}")]
    public async Task<IActionResult> ListarPorArtista(int idArtista)
    {
        if (await _artistaRepository.BuscarPorId(idArtista) is null)
            return NotFound("Artista não encontrado.");
        return Ok(await _musicaRepository.ListarPorArtista(idArtista));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cadastrar([FromBody] MusicaDTO dto)
    {
        if (await _albumRepository.BuscarPorId(dto.IdAlbum) is null)
            return BadRequest("Álbum informado não existe.");
        if (await _artistaRepository.BuscarPorId(dto.IdArtista) is null)
            return BadRequest("Artista informado não existe.");
        if (await _generoRepository.BuscarPorId(dto.IdGenero) is null)
            return BadRequest("Gênero informado não existe.");

        var musica = new Musica
        {
            Titulo = dto.Titulo,
            Duracao = dto.Duracao,
            IdAlbum = dto.IdAlbum,
            IdArtista = dto.IdArtista,
            IdGenero = dto.IdGenero
        };

        await _musicaRepository.Cadastrar(musica);
        return CreatedAtAction(nameof(BuscarPorId), new { id = musica.IdMusica }, musica);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(int id, [FromBody] MusicaDTO dto)
    {
        if (await _musicaRepository.BuscarPorId(id) is null) return NotFound();

        if (await _albumRepository.BuscarPorId(dto.IdAlbum) is null)
            return BadRequest("Álbum informado não existe.");
        if (await _artistaRepository.BuscarPorId(dto.IdArtista) is null)
            return BadRequest("Artista informado não existe.");
        if (await _generoRepository.BuscarPorId(dto.IdGenero) is null)
            return BadRequest("Gênero informado não existe.");

        await _musicaRepository.Atualizar(id, new Musica
        {
            Titulo = dto.Titulo,
            Duracao = dto.Duracao,
            IdAlbum = dto.IdAlbum,
            IdArtista = dto.IdArtista,
            IdGenero = dto.IdGenero
        });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Excluir(int id)
    {
        if (await _musicaRepository.BuscarPorId(id) is null) return NotFound();
        await _musicaRepository.Excluir(id);
        return NoContent();
    }
}
