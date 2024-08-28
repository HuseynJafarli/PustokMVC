using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;
using Pustok.Business.ViewModels.BookViewModels;
using Pustok.Core.Models;

namespace Pustok.Business.Services.Implementations
{
    public class BookService : IBookService
    {
        public Task CreateAsync(BookCreateVM vm)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int? id, BookUpdateVM vm)
        {
            throw new NotImplementedException();
        }
    }
}
