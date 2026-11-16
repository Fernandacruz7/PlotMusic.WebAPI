using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Context;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;
using PlotMusic.WebAPI.Utils;

namespace PlotMusic.WebAPI.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly PlotMusicDbContext _context;

    public UsuarioRepository(PlotMusicDbContext context)
    {
        _context = context;
    }

    public async Task Cadastrar(Usuario usuario)
    {
        usuario.Senha = Criptografia.GerarHash(usuario.Senha);
        usuario.DataCriacao = DateOnly.FromDateTime(DateTime.UtcNow);
        await _context.Usuario.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task Atualizar(int id, Usuario usuario)
    {
        var existente = await _context.Usuario.FindAsync(id);
        if (existente != null)
        {
            existente.Nome = usuario.Nome;
            existente.Email = usuario.Email;
            existente.IdPerfil = usuario.IdPerfil;
            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                existente.Senha = Criptografia.GerarHash(usuario.Senha);
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Usuario.FindAsync(id);
        if (existente != null)
        {
            _context.Usuario.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Usuario?> BuscarPorId(int id)
    {
        return await _context.Usuario
            .AsNoTracking()
            .Include(u => u.Perfil)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
    }

    public async Task<List<Usuario>> Listar()
    {
        return await _context.Usuario
            .AsNoTracking()
            .Include(u => u.Perfil)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<Usuario?> BuscarPorEmailESenha(string email, string senha)
    {
        var usuario = await _context.Usuario
            .AsNoTracking()
            .Include(u => u.Perfil)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null) return null;

        bool senhaValida = Criptografia.CompararHash(senha, usuario.Senha);
        return senhaValida ? usuario : null;
    }
}
