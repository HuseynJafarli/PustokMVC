using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.Utilities.Extensions;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;

namespace Pustok.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IAuthorService authorService;
        private readonly IGenreService genreService;
        private readonly IWebHostEnvironment env;
        private readonly IGenreRepository genreRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IBookImageRepository bookImageRepository;

        public BookController(
            IBookRepository bookRepository,
            IAuthorService authorService, 
            IGenreService genreService, 
            IWebHostEnvironment env, 
            IGenreRepository genreRepository,
            IAuthorRepository authorRepository,
            IBookImageRepository bookImageRepository)
        {
            this.bookRepository = bookRepository;
            this.authorService = authorService;
            this.genreService = genreService;
            this.env = env;
            this.genreRepository = genreRepository;
            this.authorRepository = authorRepository;
            this.bookImageRepository = bookImageRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await genreService.GetAllAsync(x => !x.IsDeleted);
            ViewBag.Authors = await authorService.GetAllAsync(x => !x.IsDeleted);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateVM vm)
        {
            ViewBag.Genres = await genreService.GetAllAsync(x => !x.IsDeleted);
            ViewBag.Authors = await authorService.GetAllAsync(x => !x.IsDeleted);

            if (!ModelState.IsValid) return View(vm);

            if (await genreRepository.Table.AllAsync(g => g.Id != vm.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre is not found");
                return View();
            }

            if (await authorRepository.Table.AllAsync(a => a.Id != vm.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author is not found");
                return View();
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
                if (vm.PosterImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("PosterImage", "File type is not correct");
                    return View();
                }
                if (vm.PosterImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("PosterImage", "File size should be less than 2mb");
                    return View();
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
            }

            if (vm.HoverImage is not null)
            {
                if (vm.HoverImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("HoverImage", "File type is not correct");
                    return View();
                }
                if (vm.HoverImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("HoverImage", "File size should be less than 2mb");
                    return View();
                }
                BookImage bookImage = new BookImage()
                {
                    ImageUrl = vm.HoverImage.SaveFile(env.WebRootPath, "uploads/books"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsPoster = true,
                    Book = book,
                };
                await bookImageRepository.CreateAsync(bookImage);
            }

            if (vm.ImageFiles.Count > 0)
            {
                foreach (var image in vm.ImageFiles)
                {
                    if (image.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageFiles", "File type is not correct");
                        return View();
                    }
                    if (image.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFiles", "File size should be less than 2mb");
                        return View();
                    }
                    BookImage bookImage = new BookImage()
                    {
                        ImageUrl = image.SaveFile(env.WebRootPath, "uploads/books"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsPoster = true,
                        Book = book,
                    };
                    await bookImageRepository.CreateAsync(bookImage);
                }
            }

            await bookRepository.CreateAsync(book);
            await bookRepository.CommitAsync();

            return RedirectToAction("Index");
        }
    }
}
