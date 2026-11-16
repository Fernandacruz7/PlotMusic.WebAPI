using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Interfaces;

public interface IPerfilRepository
{
    Task Cadastrar(Perfil perfil);
    Task Atualizar(int id, Perfil perfil);
    Task Excluir(int id);
    Task<Perfil?> BuscarPorId(int id);
    Task<List<Perfil>> Listar();
}
