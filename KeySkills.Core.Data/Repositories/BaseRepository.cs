using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeySkills.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KeySkills.Core.Data.Repositories
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;

        public BaseRepository(TContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _context.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate) =>
            await _context.Set<TEntity>().Where(e => predicate(e)).ToListAsync();

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}