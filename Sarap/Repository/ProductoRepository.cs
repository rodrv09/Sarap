using Sarap.Models;
using Microsoft.EntityFrameworkCore;

namespace Sarap.Repository
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly EspeciasSarapiquiContext _context;

        public ProveedorRepository(EspeciasSarapiquiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedore>> GetAllAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }
    }
}
