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

            int disponible = vacacion.DiasDisponibles ?? 0;
            int usados = vacacion.DiasUsados ?? 0;

            if (disponible < cantidadDias)
            {
                // Lanzar excepción específica para manejar en el controlador
                throw new InvalidOperationException("Días insuficientes disponibles");
            }

            vacacion.DiasDisponibles = disponible - cantidadDias;
            vacacion.DiasUsados = usados + cantidadDias;
            vacacion.FechaUltimaActualizacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Loggear error
                return false;
            }
        }

    }
}
