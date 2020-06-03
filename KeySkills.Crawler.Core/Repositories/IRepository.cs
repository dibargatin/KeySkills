using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeySkills.Crawler.Core.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Reads all entities
        /// </summary>
        /// <returns>Entities collection</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Reads entities matched to predicate
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns>Entities collection</returns>
        Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate);

        /// <summary>
        /// Creates entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        Task CreateAsync(T entity);

        /// <summary>
        /// Creates entities
        /// </summary>
        /// <param name="entities">Entities to create</param>
        Task CreateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates existed entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates existed entities
        /// </summary>
        /// <param name="entities">Entities to update</param>
        Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Deletes existed entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes existed entities
        /// </summary>
        /// <param name="entities">Entities to delete</param>
        Task DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Deletes existed entities matched to predicate
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        Task DeleteAsync(Func<T, bool> predicate);
    }
}