using Microsoft.EntityFrameworkCore;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels.SlideViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;

namespace Pustok.Business.Services.Implementations
{
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository _slideRepository;
        public SlideService(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
        }
        public async Task CreateAsync(SlideCreateViewModel vm)
        {
            var entity = new Slide
            {
                Title = vm.Title,
                Description = vm.Description,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await _slideRepository.CreateAsync(entity);
            await _slideRepository.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }

            _slideRepository.Delete(entity);
            await _slideRepository.CommitAsync();
        }

        public async Task<ICollection<Slide>> GetAllAsync()
        {
            return await _slideRepository.GetAll(null).ToListAsync();
        }

        public async Task<Slide> GetByIdAsync(int id)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }

            return entity;
        }

        public async Task UpdateAsync(int id, SlideUpdateViewModel vm)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }
            entity.Title = vm.Title;
            entity.Description = vm.Description;
            entity.UpdatedAt = DateTime.Now;
            await _slideRepository.CommitAsync();
        }
    }
}
