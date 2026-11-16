using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Context;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly PlotMusicDbContext _context;

    public PerfilRepository(PlotMusicDbContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Perfil perfil)
    {
        await _context.Perfil.AddAsync(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task Atualizar(int id, Perfil perfil)
    {
        var existente = await _context.Perfil.FindAsync(id);
        if (existente != null)
        {
            existente.Nome = perfil.Nome;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Perfil.FindAsync(id);
        if (existente != null)
        {
            _context.Perfil.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Perfil?> BuscarPorId(int id)
    {
        return await _context.Perfil.AsNoTracking().FirstOrDefaultAsync(p => p.IdPerfil == id);
    }

    public async Task<List<Perfil>> Listar()
    {
        return await _context.Perfil.AsNoTracking().OrderBy(p => p.Nome).ToListAsync();
    }
}
