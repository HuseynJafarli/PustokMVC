using Pustok.Business.ViewModels;
using Pustok.Business.ViewModels.SlideViewModels;
using Pustok.Core.Models;

namespace Pustok.Business.Services.Interfaces
{
    public interface ISlideService
    {
        Task CreateAsync(SlideCreateViewModel vm);
        Task UpdateAsync(int id, SlideUpdateViewModel vm);
        Task<Slide> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task<ICollection<Slide>> GetAllAsync();
    }
}
