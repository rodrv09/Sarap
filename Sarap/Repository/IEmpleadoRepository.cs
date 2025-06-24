using Sarap.Models;

namespace Sarap.Repository
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> GetAllAsync();
        Task<Empleado?> GetByIdAsync(int id);
        Task AddAsync(Empleado empleado);
        Task UpdateAsync(Empleado empleado);
        Task DeleteAsync(int id);
        Task AssignRoleAsync(int empleadoId, string role);
        Task<IEnumerable<Empleado>> SearchAsync(string term);
    }
}
