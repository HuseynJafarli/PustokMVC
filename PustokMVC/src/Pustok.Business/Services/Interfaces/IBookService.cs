using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(BookCreateVM vm);
        Task UpdateAsync(int? id, BookUpdateVM vm);
        Task<Book> GetByIdAsync(int? id);

        Task DeleteAsync(int? id);

        Task<ICollection<Book>> GetAllAsync(Expression<Func<Book, bool>> expression, params string[] includes);
        Task<ICollection<Book>> GetAllByOrderAsync(Expression<Func<Book, bool>> expression, Expression<Func<Book, dynamic>> orderExpression, params string[] includes);
        Task<bool> IsExist(Expression<Func<Book, bool>> expression);
        Task<Book> GetByExpressionAsync(Expression<Func<Book, bool>> expression, params string[] includes);

    }
}
