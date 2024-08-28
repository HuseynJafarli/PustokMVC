using Pustok.Business.ViewModels;
using Pustok.Business.ViewModels.BookViewModels;
using Pustok.Core.Models;

namespace Pustok.Business.Services.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(BookCreateVM vm);
        Task UpdateAsync(int? id, BookUpdateVM vm);
        Task<Book> GetByIdAsync(int? id);

        Task DeleteAsync(int id);

        Task<ICollection<Book>> GetAllAsync();
    }
}
