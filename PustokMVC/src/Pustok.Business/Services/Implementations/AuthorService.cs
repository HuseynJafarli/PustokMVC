using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepo;

        public AuthorService(IAuthorRepository authorRepo)
        {
            _authorRepo = authorRepo;
        }
        public async Task CreateAsync(AuthorCreateVM vm)
        {
            if (string.IsNullOrEmpty(vm.FullName))
            {
                throw new AuthorFullNameException("Fullname", "Author fullname can not be empty");
            }
            var data = new Author()
            {

                FullName = vm.FullName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false,

            };
            await _authorRepo.CreateAsync(data);
            await _authorRepo.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _authorRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new EntityNotFoundException("Author not found");
            }
            _authorRepo.Delete(data);
            await _authorRepo.CommitAsync();
        }

        public async Task<ICollection<Author>> GetAllAsync(Expression<Func<Author, bool>> expression)
        {
            return await _authorRepo.GetAll(expression).ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int? id)
        {
            if (id <= 0 || id == null)
            {
                throw new IdIsNotValid("Id is not valid");
            }
            return await _authorRepo.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int? id, AuthorUpdateVM vm)
        {
            if (id <= 0 || id == null)
            {
                throw new IdIsNotValid("Id is not valid");
            }
            if (string.IsNullOrEmpty(vm.FullName))
            {
                throw new AuthorFullNameException("Fullname", "Author fullname can not be empty");
            }

            var data = await _authorRepo.GetByIdAsync(id);
            if (data is null)
            {
                throw new EntityNotFoundException("Author not found");
            }

            data.FullName = vm.FullName;
            data.UpdatedAt = DateTime.Now;

            await _authorRepo.CommitAsync();
        }
    }
}
