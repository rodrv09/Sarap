using Sarap.Models;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<bool> UserExists(string nombreUsuario, string email);
        Task<Usuario?> GetByUsernameAsync(string nombreUsuario);
    }
}