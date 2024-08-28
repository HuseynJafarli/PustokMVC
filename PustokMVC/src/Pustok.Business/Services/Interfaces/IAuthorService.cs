using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Interfaces
{
    public interface IAuthorService
    {
        Task CreateAsync(AuthorCreateVM vm);
        Task UpdateAsync(int? id, AuthorUpdateVM vm);
        Task<Author> GetByIdAsync(int? id);

        Task DeleteAsync(int id);

        Task<ICollection<Author>> GetAllAsync(Expression<Func<Author, bool>> expression);
    }
}
