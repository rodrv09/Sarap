using Sarap.Models;

namespace Sarap.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ReadAsync();
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> CreateAsync(Usuario usuario);
        Task<bool> DeleteAsync(Usuario usuario);
        Task<bool> ActivarAsync(Usuario usuario);
        Task<bool> DesactivarAsync(Usuario usuario);
    }
}
