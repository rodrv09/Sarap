using Sarap.Models;

namespace Repository
{
    /// <summary>
    /// Repositorio especializado para la entidad Empleado.
    /// Hereda de RepositoryBase<Empleado> para usar los métodos CRUD genéricos.
    /// </summary>
    public class EmpleadoRepository : RepositoryBase<Empleado>
    {
        public EmpleadoRepository() : base() { }

    }
}
