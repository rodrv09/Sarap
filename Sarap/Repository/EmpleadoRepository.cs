using Sarap.Models;
using Microsoft.EntityFrameworkCore;

namespace Sarap.Repository
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly EspeciasSarapiquiContext _context;

        public EmpleadoRepository(EspeciasSarapiquiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleado
                .Include(e => e.Usuario)
                .Where(e => e.Activo)
                .ToListAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleado
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Empleado empleado)
        {
            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Empleado empleado)
        {
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado != null)
            {
                empleado.Activo = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignRoleAsync(int empleadoId, string role)
        {
            var empleado = await _context.Empleado.FindAsync(empleadoId);
            if (empleado != null)
            {
                empleado.Rol = role;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Empleado>> SearchAsync(string term)
        {
            return await _context.Empleado
                .Where(e => e.Activo &&
                    (e.Nombre.Contains(term) ||
                     e.Apellidos.Contains(term) ||
                     e.Cedula.Contains(term) ||
                     e.Rol.Contains(term)))
                .ToListAsync();
        }
    }
}
