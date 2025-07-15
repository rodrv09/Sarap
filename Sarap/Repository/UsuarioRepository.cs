using CAAP2.Architecture.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sarap.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository() : base() { }

        public async Task<bool> UserExists(string nombreUsuario, string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == nombreUsuario || u.Email == email);
        }

        public async Task<Usuario?> GetByUsernameAsync(string nombreUsuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        // Método para generar hash SHA256
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashedInput = HashPassword(inputPassword);
            return string.Equals(hashedInput, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}