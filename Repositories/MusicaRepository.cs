using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Context;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Repositories;

public class MusicaRepository : IMusicaRepository
{
    private readonly PlotMusicDbContext _context;

    public MusicaRepository(PlotMusicDbContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Musica musica)
    {
        await _context.Musica.AddAsync(musica);
        await _context.SaveChangesAsync();
    }

    public async Task Atualizar(int id, Musica musica)
    {
        var existente = await _context.Musica.FindAsync(id);
        if (existente != null)
        {
            existente.Titulo = musica.Titulo;
            existente.Duracao = musica.Duracao;
            existente.IdAlbum = musica.IdAlbum;
            existente.IdArtista = musica.IdArtista;
            existente.IdGenero = musica.IdGenero;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Musica.FindAsync(id);
        if (existente != null)
        {
            _context.Musica.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Musica?> BuscarPorId(int id)
    {
        return await _context.Musica
            .AsNoTracking()
            .Include(m => m.Album)
            .Include(m => m.Artista)
            .Include(m => m.Genero)
            .FirstOrDefaultAsync(m => m.IdMusica == id);
    }

    public async Task<(List<Musica> items, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null)
    {
        var query = _context.Musica
            .AsNoTracking()
            .Include(m => m.Album)
            .Include(m => m.Artista)
            .Include(m => m.Genero)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
            query = query.Where(m => m.Titulo != null && m.Titulo.Contains(busca));

        int total = await query.CountAsync();

        var items = await query
            .OrderBy(m => m.Titulo).ThenBy(m => m.IdMusica)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }

    public async Task<List<Musica>> ListarPorAlbum(int idAlbum)
    {
        return await _context.Musica
            .AsNoTracking()
            .Include(m => m.Artista)
            .Include(m => m.Genero)
            .Where(m => m.IdAlbum == idAlbum)
            .OrderBy(m => m.Titulo)
            .ToListAsync();
    }

    public async Task<List<Musica>> ListarPorArtista(int idArtista)
    {
        return await _context.Musica
            .AsNoTracking()
            .Include(m => m.Album)
            .Include(m => m.Genero)
            .Where(m => m.IdArtista == idArtista)
            .OrderBy(m => m.Titulo)
            .ToListAsync();
    }
}
