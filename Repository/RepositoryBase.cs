

using CAAP2.Architecture.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sarap.Models;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

/// <summary>
/// Interface for basic repository operations.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepositoryBase<T>
{
    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> CreateAsync(T entity);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// Reads all entities of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    Task<IEnumerable<T>> ReadAsync();

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// Updates multiple entities asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> UpdateManyAsync(IEnumerable<T> entities);

    /// <summary>
    /// Checks if an entity exists asynchronously.
    /// </summary>
    /// <param name="entity">The entity to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
    Task<bool> ExistsAsync(T entity);
}

/// <summary>
/// Base class for repository operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly EspeciasSarapiquiContext _context;
    protected EspeciasSarapiquiContext DbContext => _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    public RepositoryBase()
    {
        _context = new EspeciasSarapiquiContext();
    }

    /// <summary>
    /// Creates an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be saved in the database.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> CreateAsync(T entity)
    {
        try
        {
            // Verificar que el entity tiene los datos correctos
            Console.WriteLine($"Creando entidad: {entity}");
            await _context.Set<T>().AddAsync(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            // Loguear el error con detalles completos
            Console.WriteLine("💥 Error al crear entidad: " + ex.Message);
            throw new PAWException(ex); // Lanza el error original
        }
    }




    /// <summary>
    /// Updates an existing entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _context.Update(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Updates multiple entities of type T asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
    {
        try
        {
            _context.UpdateRange(entities);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Deletes an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            _context.Remove(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Reads all entities of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<IEnumerable<T>> ReadAsync()
    {
        try
        {
            return await _context.Set<T>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Checks if an entity of type T exists asynchronously.
    /// </summary>
    /// <param name="entity">The entity to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
    public async Task<bool> ExistsAsync(T entity)
    {
        try
        {
            var items = await ReadAsync();
            return items.Any(x => x.Equals(entity));
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    /// <summary>
    /// Saves changes to the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> SaveAsync()
    {
        try
        {
            // Este método persiste todos los cambios realizados en el contexto
            int changes = await _context.SaveChangesAsync();
            return changes > 0;  // Si se han realizado cambios, se devuelve true
        }
        catch (Exception ex)
        {
            // Si hay algún error, se lanza una excepción personalizada
            throw new PAWException("Error saving changes to the database", ex);
        }
    }

    internal async Task<bool> CreateAsync(Component component)
    {
        throw new NotImplementedException();
    }
}
