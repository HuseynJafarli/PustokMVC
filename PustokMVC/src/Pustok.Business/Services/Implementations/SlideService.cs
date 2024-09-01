using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.Utilities.Extensions;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Implementations
{
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IWebHostEnvironment env;

        public SlideService(ISlideRepository slideRepository, IWebHostEnvironment env)
        {
            _slideRepository = slideRepository;
            this.env = env;
        }
        public async Task CreateAsync(SlideCreateViewModel vm)
        {
            if (vm.Photo != null)
            {
                if (!vm.Photo.ContentType.StartsWith("image/"))
                {
                    throw new FileValidationException("Photo", "File type is not correct");
                }
                if (vm.Photo.Length > 2 * 1024 * 1024)
                {
                    throw new FileValidationException("Photo", "File size should be less than 2mb");
                }

                var entity = new Slide
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Image = vm.Photo.SaveFile(env.WebRootPath, "uploads/slides")
                };
                await _slideRepository.CreateAsync(entity);
                await _slideRepository.CommitAsync();
            }
        }

        public async Task DeleteAsync(int? id)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new IdIsNotValid();
            }
            entity.Image.DeleteFile(env.WebRootPath, "uploads", "slides");

            _slideRepository.Delete(entity);
            await _slideRepository.CommitAsync();
        }

        public async Task<ICollection<Slide>> GetAllAsync(Expression<Func<Slide, bool>>? expression = null)
        {
           return await _slideRepository.GetAll(expression).ToListAsync();
        }

        public async Task<Slide> GetByIdAsync(int? id)
        {
            var entity = await _slideRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }

            return entity;
        }

        public async Task UpdateAsync(int? id, SlideUpdateViewModel slideVM)
        {
            var entity = await _slideRepository.GetByIdAsync(id) ?? throw new IdIsNotValid("Id is not valid");

            entity.Title = slideVM.Title;
            entity.Description = slideVM.Description;
            entity.UpdatedAt = DateTime.Now;

            if (slideVM.Photo != null)
            {
                entity.Image.DeleteFile(env.WebRootPath, "uploads", "slides");
                entity.Image = slideVM.Photo.SaveFile(env.WebRootPath, "uploads/slides");
            }

            await _slideRepository.CommitAsync();
        }
    }
}
