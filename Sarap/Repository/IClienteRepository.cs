using Sarap.Models;

namespace Sarap.Repository
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
    }
}
