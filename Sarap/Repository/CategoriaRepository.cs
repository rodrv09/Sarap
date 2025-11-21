using Sarap.Models;

namespace Repository
{
    /// <summary>
    /// Repositorio especializado para la entidad Cliente.
    /// Hereda de RepositoryBase<Cliente> para usar los métodos CRUD genéricos.
    /// </summary>
    public class CategoriaRepository : RepositoryBase<Categoria>
    {
        public CategoriaRepository() : base() { }


    }

}