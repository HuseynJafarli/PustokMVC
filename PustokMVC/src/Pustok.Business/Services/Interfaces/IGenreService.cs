using Pustok.Business.ViewModels.GenreViewModels;
using Pustok.Core.Models;

namespace Pustok.Business.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(GenreCreateViewModel vm);
        Task UpdateAsync(int id, GenreUpdateViewModel vm);
        Task<Genre> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task<ICollection<Genre>> GetAllAsync();
    }
}
