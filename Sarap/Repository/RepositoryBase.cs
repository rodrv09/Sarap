using CAAP2.Architecture.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sarap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Interface for basic repository operations.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepositoryBase<T>
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IEnumerable<T>> ReadAsync();
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateManyAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(T entity);
        Task<bool> ActivarAsync(T entity);
        Task<bool> DesactivarAsync(T entity);
    }

    /// <summary>
    /// Base class for repository operations.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly EspeciasSarapiquiContext _context;
        protected EspeciasSarapiquiContext DbContext => _context;

        public RepositoryBase()
        {
            _context = new EspeciasSarapiquiContext();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                Console.WriteLine($"Creando entidad: {entity}");
                await _context.Set<T>().AddAsync(entity);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("💥 Error al crear entidad: " + ex.Message);
                throw new PAWException(ex);
            }
        }

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

        public async Task<IEnumerable<T>> ReadAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en ReadAsync:");
                Console.WriteLine(ex.ToString());  // Imprime stack trace completo en consola
                throw new PAWException(ex);
            }
        }



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

        public async Task<bool> SaveAsync()
        {
            try
            {
                int changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                throw new PAWException("Error saving changes to the database", ex);
            }
        }

        /// <summary>
        /// Activa una entidad (establece Activo = true).
        /// </summary>
        public async Task<bool> ActivarAsync(T entity)
        {
            try
            {
                var prop = typeof(T).GetProperty("Activo");
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(entity, true);
                    _context.Update(entity);
                    return await SaveAsync();
                }
                else
                {
                    throw new InvalidOperationException("La entidad no tiene una propiedad 'Activo' de tipo booleano.");
                }
            }
            catch (Exception ex)
            {
                throw new PAWException("Error al activar la entidad", ex);
            }
        }

        /// <summary>
        /// Desactiva una entidad (establece Activo = false).
        /// </summary>
        public async Task<bool> DesactivarAsync(T entity)
        {
            try
            {
                var prop = typeof(T).GetProperty("Activo");
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(entity, false);
                    _context.Update(entity);
                    return await SaveAsync();
                }
                else
                {
                    throw new InvalidOperationException("La entidad no tiene una propiedad 'Activo' de tipo booleano.");
                }
            }
            catch (Exception ex)
            {
                throw new PAWException("Error al desactivar la entidad", ex);
            }
        }

        internal async Task<bool> CreateAsync(System.ComponentModel.Component component)
        {
            throw new NotImplementedException();
        }
    }
}
