using Microsoft.EntityFrameworkCore;
using Sarap.Models;

namespace Sarap.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EspeciasSarapiquiContext _context;

        public UsuarioRepository(EspeciasSarapiquiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> ReadAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActivarAsync(Usuario usuario)
        {
            usuario.Activo = true;
            _context.Entry(usuario).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DesactivarAsync(Usuario usuario)
        {
            usuario.Activo = false;
            _context.Entry(usuario).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
