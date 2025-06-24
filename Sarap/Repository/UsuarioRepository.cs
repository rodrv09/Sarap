using Sarap.Models;

namespace Repository;

/// <summary>
/// Repositorio especializado para la entidad Usuario.
/// Hereda de RepositoryBase<Usuario> para usar los métodos CRUD genéricos.
/// </summary>
public class UsuarioRepository : RepositoryBase<Usuario>
{
    public UsuarioRepository() : base() { }

    // Aquí puedes agregar métodos personalizados para usuarios si los necesitas.
    // Por ejemplo:
    // public Task<Usuario> BuscarPorEmail(string email) { ... }
}
