using Sarap.Models;

namespace Sarap.Repository
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Proveedore>> GetAllAsync();
    }
}
