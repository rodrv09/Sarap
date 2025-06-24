

using Sarap.Models;

namespace Repository
{
    /// <summary>
    /// Repositorio especializado para la entidad Producto.
    /// Hereda de RepositoryBase<Producto> para usar los métodos CRUD genéricos.
    /// </summary>
    public class ProductoRepository : RepositoryBase<Producto>
    {
        public ProductoRepository() : base() { }
    }
}
