using Microsoft.EntityFrameworkCore;
using Sarap.Models;
using System.Threading.Tasks;

namespace Repository
{
    public class VacacionesEmpleadoRepository : RepositoryBase<VacacionesEmpleado>
    {
        public VacacionesEmpleadoRepository() : base() { }

        // Obtener vacaciones por Id (clave primaria)
        public async Task<VacacionesEmpleado> GetByIdAsync(int id)
        {
            return await _context.VacacionesEmpleado.FindAsync(id);
        }

        // Obtener vacaciones por Identidad (string)
        public async Task<VacacionesEmpleado> GetByIdentidadAsync(string identidad)
        {
            return await _context.VacacionesEmpleado
                .FirstOrDefaultAsync(v => v.Identidad == identidad);
        }

        // Solicitar días: suma a DiasUsados y resta de DiasDisponibles
        public async Task<bool> SolicitarDiasAsync(int id, int cantidadDias)
        {
            var vacacion = await GetByIdAsync(id);
            if (vacacion == null) return false;

            if (cantidadDias <= 0) return false;

            // Si DiasDisponibles es null, consideramos 0 para evitar excepción
            int disponible = vacacion.DiasDisponibles ?? 0;
            int usados = vacacion.DiasUsados ?? 0;

            if (disponible < cantidadDias) return false;

            vacacion.DiasDisponibles = disponible - cantidadDias;
            vacacion.DiasUsados = usados + cantidadDias;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Puedes loguear el error aquí
                return false;
            }
        }

    }
}
