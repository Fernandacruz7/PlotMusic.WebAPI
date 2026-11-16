using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;
using PlotMusic.WebAPI.Services;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistaRepository _artistaRepository;
    private readonly IImageService _imageService;

    public AlbumController(IAlbumRepository albumRepository, IArtistaRepository artistaRepository, IImageService imageService)
    {
        _albumRepository = albumRepository;
        _artistaRepository = artistaRepository;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<IActionResult> Lista(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        [FromQuery] string? busca = null)
    {
        if (pagina < 1 || tamanhoPagina is < 1 or > 100)
            return BadRequest("Página deve ser positiva e tamanhoPagina deve estar entre 1 e 100.");

        var (items, total) = await _albumRepository.ListarPaginado(pagina, tamanhoPagina, busca);
        return Ok(new { items, total, pagina, tamanhoPagina });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var album = await _albumRepository.BuscarPorId(id);
        return album is null ? NotFound() : Ok(album);
    }

    [HttpGet("artista/{idArtista:int}")]
    public async Task<IActionResult> ListarPorArtista(int idArtista)
    {
        if (await _artistaRepository.BuscarPorId(idArtista) is null)
            return NotFound("Artista não encontrado.");
        var albums = await _albumRepository.ListarPorArtista(idArtista);
        return Ok(albums);
    }

    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(11 * 1024 * 1024)]
    public async Task<IActionResult> Cadastrar([FromForm] AlbumDTO dto)
    {
        if (await _artistaRepository.BuscarPorId(dto.IdArtista) is null)
            return BadRequest("Artista informado não existe.");

        if (dto.ArquivoImagem is not null && !_imageService.IsConfigured)
            return Problem("Configure as credenciais do Cloudinary para enviar imagens.", statusCode: 503);

        string? novaUrl = null;
        try
        {
            if (dto.ArquivoImagem is not null)
                novaUrl = await _imageService.UploadImageAsync(dto.ArquivoImagem, ImageFolder.Albums);
        }
        catch (ArgumentException erro) { return BadRequest(erro.Message); }
        catch (HttpRequestException) { return Problem("Falha ao enviar imagem ao Cloudinary.", statusCode: 502); }

        var album = new Album
        {
            Titulo = dto.Titulo,
            AnoLancamento = dto.AnoLancamento,
            IdArtista = dto.IdArtista,
            ImagemURL = novaUrl ?? (string.IsNullOrWhiteSpace(dto.ImagemURL) ? null : dto.ImagemURL)
        };

        try
        {
            await _albumRepository.Cadastrar(album);
        }
        catch
        {
            if (novaUrl is not null) await _imageService.DeleteImageAsync(novaUrl);
            throw;
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = album.IdAlbum }, album);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(11 * 1024 * 1024)]
    public async Task<IActionResult> Atualizar(int id, [FromForm] AlbumDTO dto)
    {
        var atual = await _albumRepository.BuscarPorId(id);
        if (atual is null) return NotFound();

        if (await _artistaRepository.BuscarPorId(dto.IdArtista) is null)
            return BadRequest("Artista informado não existe.");

        if (dto.ArquivoImagem is not null && !_imageService.IsConfigured)
            return Problem("Configure as credenciais do Cloudinary para enviar imagens.", statusCode: 503);

        string? novaUrl = null;
        try
        {
            if (dto.ArquivoImagem is not null)
                novaUrl = await _imageService.UploadImageAsync(dto.ArquivoImagem, ImageFolder.Albums);
        }
        catch (ArgumentException erro) { return BadRequest(erro.Message); }
        catch (HttpRequestException) { return Problem("Falha ao enviar imagem ao Cloudinary.", statusCode: 502); }

        var imagem = novaUrl ?? (string.IsNullOrWhiteSpace(dto.ImagemURL) ? atual.ImagemURL : dto.ImagemURL);

        try
        {
            await _albumRepository.Atualizar(id, new Album
            {
                Titulo = dto.Titulo,
                AnoLancamento = dto.AnoLancamento,
                IdArtista = dto.IdArtista,
                ImagemURL = imagem
            });
        }
        catch
        {
            if (novaUrl is not null) await _imageService.DeleteImageAsync(novaUrl);
            throw;
        }

        if (!string.IsNullOrWhiteSpace(atual.ImagemURL) && atual.ImagemURL != imagem)
            await _imageService.DeleteImageAsync(atual.ImagemURL);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Excluir(int id)
    {
        var album = await _albumRepository.BuscarPorId(id);
        if (album is null) return NotFound();
        await _albumRepository.Excluir(id);
        if (!string.IsNullOrWhiteSpace(album.ImagemURL))
            await _imageService.DeleteImageAsync(album.ImagemURL);
        return NoContent();
    }
}
