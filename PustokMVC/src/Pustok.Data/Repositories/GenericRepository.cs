using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;
using System.Linq.Expressions;

namespace Pustok.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (expression is not null)
            {
                return query.Where(expression);
            }
            else
            {
                return query;
            }
        }

        public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (expression.Body != null) 
            {
                return await query.Where(expression).FirstOrDefaultAsync();
            }
            else
            {
                 return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<TEntity> GetByIdAsync(int? id, params string[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (includes.Length > 0) 
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
