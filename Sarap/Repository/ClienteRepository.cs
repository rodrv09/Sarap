using Sarap.Models;
using Microsoft.EntityFrameworkCore;

namespace Sarap.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly EspeciasSarapiquiContext _context;

        public ClienteRepository(EspeciasSarapiquiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
    }
}
