using Pustok.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Core.Repositories
{
    public interface GenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id, params string[] includes);
        Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity,bool>> expression, params string[] includes);

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes);
        void Delete(TEntity entity);
        Task<int> CommitAsync();

    }
}
