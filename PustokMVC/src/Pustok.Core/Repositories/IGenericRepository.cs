using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public DbSet<TEntity> Table { get; }
        Task CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int? id, params string[] includes);
        Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity,bool>> expression, params string[] includes);

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes);
        void Delete(TEntity entity);
        Task<int> CommitAsync();

    }
}
