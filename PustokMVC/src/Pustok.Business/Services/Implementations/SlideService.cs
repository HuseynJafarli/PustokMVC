using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels.GenreViewModels;
using Pustok.Business.ViewModels.SlideViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Order = vm.Order,
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

        public async Task<ICollection<Genre>> GetAllAsync()
        {
            return await _slideRepository.GetAll(null).ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }

            return entity;
        }

        public async Task UpdateAsync(int id, GenreUpdateViewModel vm)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }
            entity.Name = vm.Name;
            entity.UpdatedAt = DateTime.Now;
            await _slideRepository.CommitAsync();
        }
    }
}
