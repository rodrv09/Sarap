using Sarap.Models;

namespace Repository
{
    /// <summary>
    /// Repositorio especializado para la entidad Cliente.
    /// Hereda de RepositoryBase<Cliente> para usar los métodos CRUD genéricos.
    /// </summary>
    public class  ProveedorRepository : RepositoryBase<Proveedore>
    {
        public ProveedorRepository() : base() { }

        // Aquí puedes agregar métodos personalizados para Cliente si lo necesitas.
        // Ejemplo:
        // public Cliente? BuscarPorEmail(string email) => Context.Clientes.FirstOrDefault(c => c.Email == email);
    }
}
