using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KeySkills.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Repositories
{
    /// <summary>
    /// Base implementation of <see cref="IRepository"/>
    /// </summary>
    /// <typeparam name="TEntity">Entity type of the repository</typeparam>
    /// <typeparam name="TContext">Type of DbContext used for the repository</typeparam>
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _context;

        /// <summary>
        /// Initializes repository
        /// </summary>
        /// <param name="context">Instance of DbContext for repository</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/></exception>
        public BaseRepository(TContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));

        /// <inheritdoc/>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _context.Set<TEntity>().ToListAsync();

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _context.Set<TEntity>().Where(predicate).ToListAsync();

        /// <inheritdoc/>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().AnyAsync(predicate);
    }
}