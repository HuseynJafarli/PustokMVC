using Microsoft.EntityFrameworkCore;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels.GenreViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;

namespace Pustok.Business.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task CreateAsync(GenreCreateViewModel vm)
        {
            var entity = new Genre
            {
                Name = vm.Name,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await _genreRepository.CreateAsync(entity);
            await _genreRepository.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }

            _genreRepository.Delete(entity);
            await _genreRepository.CommitAsync();
        }

        public async Task<ICollection<Genre>> GetAllAsync()
        {
            return await _genreRepository.GetAll(null).ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if(entity is null)
            {
                throw new NullReferenceException();
            }

            return entity;
        }

        public async Task UpdateAsync(int id, GenreUpdateViewModel vm)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity is null)
            {
                throw new NullReferenceException();
            }
            entity.Name = vm.Name;
            entity.UpdatedAt = DateTime.Now;
            await _genreRepository.CommitAsync();
        }
    }
}
