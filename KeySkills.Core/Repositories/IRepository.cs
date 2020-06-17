using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KeySkills.Core.Repositories
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
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Adds entities
        /// </summary>
        /// <param name="entities">Entities to add</param>
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates existed entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes existed entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        Task DeleteAsync(T entity);
        
        /// <summary>
        /// Checks whether any entity matched to predicate exist or not
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <returns><see langword="true"/> if any entity exists or <see langword="false"/> otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}