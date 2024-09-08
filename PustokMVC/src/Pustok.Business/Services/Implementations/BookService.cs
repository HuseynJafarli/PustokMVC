using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;
        private readonly IWebHostEnvironment env;
        private readonly IGenreRepository genreRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IBookImageRepository bookImageRepository;

        public BookService(IBookRepository bookRepository,
            IWebHostEnvironment env,
            IGenreRepository genreRepository,
            IAuthorRepository authorRepository,
            IBookImageRepository bookImageRepository)
        {
            this.bookRepository = bookRepository;
            this.env = env;
            this.genreRepository = genreRepository;
            this.authorRepository = authorRepository;
            this.bookImageRepository = bookImageRepository;
        }
        public async Task CreateAsync(BookCreateVM vm)
        {
            if (await genreRepository.Table.AllAsync(g => g.Id != vm.GenreId))
            {
                throw new EntityNotFoundException("GenreId", "Genre is not found");
            }

            if (await authorRepository.Table.AllAsync(a => a.Id != vm.AuthorId))
            {
                throw new EntityNotFoundException("AuthorId", "Author is not found");

            }


            Book book = new Book()
            {
                Title = vm.Title,
                Desc = vm.Desc,
                StockCount = vm.StockCount,
                SalePrice = vm.SalePrice,
                CostPrice = vm.CostPrice,
                Discount = vm.Discount,
                IsAvalible = vm.IsAvailable,
                IsDeleted = false,
                ProductCode = vm.ProductCode,
                AuthorId = vm.AuthorId,
                GenreId = vm.GenreId,
            };

            if (vm.PosterImage != null)
            {
                if (!vm.PosterImage.ContentType.StartsWith("image/"))
                {
                    throw new FileValidationException("PosterImage", "File type is not correct");
                }
                if (vm.PosterImage.Length > 2 * 1024 * 1024)
                {
                    throw new FileValidationException("PosterImage", "File size should be less than 2mb");
                }

                BookImage bookImage = new BookImage()
                {
                    ImageUrl = vm.PosterImage.SaveFile(env.WebRootPath, "uploads/books"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsPoster = true,
                    Book = book,
                };
                await bookImageRepository.CreateAsync(bookImage);

                if (vm.HoverImage is not null)
                {
                    if (!vm.HoverImage.ContentType.StartsWith("image/"))
                    {
                        throw new FileValidationException("HoverImage", "File type is not correct");
                    }
                    if (vm.HoverImage.Length > 2 * 1024 * 1024)
                    {
                        throw new FileValidationException("HoverImage", "File size should be less than 2mb");
                    }
                    BookImage bookImage2 = new BookImage()
                    {
                        ImageUrl = vm.HoverImage.SaveFile(env.WebRootPath, "uploads/books"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsPoster = false,
                        Book = book,
                    };
                    await bookImageRepository.CreateAsync(bookImage2);

                    if (vm.ImageFiles.Count > 0)
                    {
                        foreach (var image in vm.ImageFiles)
                        {
                            if (!image.ContentType.StartsWith("image/"))
                            {
                                throw new FileValidationException("ImageFiles", "File type is not correct");
                            }
                            if (image.Length > 2 * 1024 * 1024)
                            {
                                throw new FileValidationException("ImageFiles", "File size should be less than 2mb");
                            }
                            BookImage bookImage3 = new BookImage()
                            {
                                ImageUrl = image.SaveFile(env.WebRootPath, "uploads/books"),
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                IsDeleted = false,
                                IsPoster = null,
                                Book = book,
                            };
                            await bookImageRepository.CreateAsync(bookImage3);
                        }
                    }
                }
            }

            await bookRepository.CreateAsync(book);
            await bookRepository.CommitAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            Book data = await bookRepository.GetByIdAsync(id, "BookImages", "Author", "Genre") ?? throw new IdIsNotValid();

            foreach (var item in data.BookImages)
            {
                item.ImageUrl.DeleteFile(env.WebRootPath, "uploads", "books");
            }
            bookRepository.Delete(data);
            await bookRepository.CommitAsync();
        }

        public async Task<ICollection<Book>> GetAllAsync(Expression<Func<Book, bool>> expression, params string[] includes)
        {
            return await bookRepository.GetAll(expression, includes).ToListAsync();
        }

        public async Task<ICollection<Book>> GetAllByOrderAsync(Expression<Func<Book, bool>> expression, Expression<Func<Book, dynamic>> orderExpression, params string[] includes)
        {
            return await bookRepository.GetAll(expression, includes).OrderByDescending(orderExpression).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int? id)
        {
            if (id <= 0 || id == null)
            {
                throw new IdIsNotValid("Id is not valid");
            }
            return await bookRepository.GetByIdAsync(id);
        }

        public Task<bool> IsExist(Expression<Func<Book, bool>> expression)
        {
            return bookRepository.Table.AnyAsync(expression);
        }

        public async Task UpdateAsync(int? id, BookUpdateVM bookVM)
        {
            Book existBook = await bookRepository.GetByExpressionAsync(x => x.Id == id, "BookImages", "Author", "Genre") ?? throw new IdIsNotValid();
            bookVM.BookImages = existBook.BookImages;


            if (await genreRepository.Table.AllAsync(g => g.Id != bookVM.GenreId))
            {
                throw new EntityNotFoundException("GenreId", "Genre is not found");
            }

            if (await authorRepository.Table.AllAsync(a => a.Id != bookVM.AuthorId))
            {
                throw new EntityNotFoundException("AuthorId", "Author is not found");

            }

            if (bookVM.PosterImage != null)
            {
                if (!bookVM.PosterImage.ContentType.StartsWith("image/"))
                {
                    throw new FileValidationException("PosterImage", "File type is not correct");
                }
                if (bookVM.PosterImage.Length > 2 * 1024 * 1024)
                {
                    throw new FileValidationException("PosterImage", "File size should be less than 2mb");
                }
            }

            if (bookVM.HoverImage is not null)
            {
                if (!bookVM.HoverImage.ContentType.StartsWith("image/"))
                {
                    throw new FileValidationException("HoverImage", "File type is not correct");
                }
                if (bookVM.HoverImage.Length > 2 * 1024 * 1024)
                {
                    throw new FileValidationException("HoverImage", "File size should be less than 2mb");
                }
            }




            if (bookVM.PosterImage is not null)
            {
                BookImage mainImage = new BookImage()
                {
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    UpdatedAt = DateTime.Now,
                    IsPoster = true,
                    ImageUrl = bookVM.PosterImage.SaveFile(env.WebRootPath, "uploads/books")
                };

                BookImage oldPoster = existBook.BookImages.FirstOrDefault(x => x.IsPoster == true);
                oldPoster.ImageUrl.DeleteFile(env.WebRootPath, "uploads", "books");
                existBook.BookImages.Remove(oldPoster);
                existBook.BookImages.Add(mainImage);
            }


            if (bookVM.HoverImage is not null)
            {
                BookImage hoverImage = new BookImage()
                {
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    UpdatedAt = DateTime.Now,
                    IsPoster = false,
                    ImageUrl = bookVM.HoverImage.SaveFile(env.WebRootPath, "uploads/books")
                };
                BookImage oldPoster = existBook.BookImages.FirstOrDefault(x => x.IsPoster == true);
                oldPoster.ImageUrl.DeleteFile(env.WebRootPath, "uploads", "books");
                existBook.BookImages.Remove(oldPoster);
                existBook.BookImages.Add(hoverImage);
            }

            if (bookVM.BookImageIds is null)
            {
                bookVM.BookImageIds = [];
            }

            List<BookImage> deleteImages = existBook.BookImages.Where(pi => !bookVM.BookImageIds.Exists(tId => tId == pi.Id) && pi.IsPoster == null).ToList();
            foreach (BookImage item in deleteImages)
            {
                item.ImageUrl.DeleteFile(env.WebRootPath, "uploads", "books");
                existBook.BookImages.Remove(item);
            }


            if (bookVM.ImageFiles is not null)
            {

                foreach (IFormFile image in bookVM.ImageFiles)
                {
                    if (!image.ContentType.StartsWith("image/"))
                    {
                        throw new FileValidationException("ImageFiles", "File type is not correct");
                    }
                    if (image.Length > 2 * 1024 * 1024)
                    {
                        throw new FileValidationException("ImageFiles", "File size should be less than 2mb");
                    }
                    BookImage additionalPhoto = new BookImage()
                    {
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        UpdatedAt = DateTime.Now,
                        IsPoster = null,
                        ImageUrl = image.SaveFile(env.WebRootPath, "uploads/books")
                    };

                    existBook.BookImages.Add(additionalPhoto);
                }
            }


            existBook.UpdatedAt = DateTime.Now;
            existBook.Desc = bookVM.Desc;
            existBook.Title = bookVM.Title;
            existBook.AuthorId = bookVM.AuthorId;
            existBook.GenreId = bookVM.GenreId;
            existBook.CostPrice = bookVM.CostPrice;
            existBook.SalePrice = bookVM.SalePrice;
            existBook.StockCount = bookVM.StockCount;
            existBook.Discount = bookVM.Discount;
            existBook.ProductCode = bookVM.ProductCode;

            await bookRepository.CommitAsync();
        }

        public async Task<Book> GetByExpressionAsync(Expression<Func<Book, bool>> expression, params string[] includes)
        {
            Book data = await bookRepository.GetByExpressionAsync(expression, includes) ?? throw new IdIsNotValid();
            return data;
        }
    }
}
