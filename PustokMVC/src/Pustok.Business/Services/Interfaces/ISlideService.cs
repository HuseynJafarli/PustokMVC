using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Interfaces
{
    public interface ISlideService
    {
        Task CreateAsync(SlideCreateViewModel vm);
        Task UpdateAsync(int? id, SlideUpdateViewModel vm);
        Task<Slide> GetByIdAsync(int? id);
        Task DeleteAsync(int? id);

        Task<ICollection<Slide>> GetAllAsync(Expression<Func<Slide, bool>>? expression = null);
    }
}
