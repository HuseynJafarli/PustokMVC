using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions;
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
        private readonly IAuthorService authorService;
        private readonly IGenreService genreService;
        private readonly IBookService bookService;

        public BookController(
            IAuthorService authorService, 
            IGenreService genreService,
            IBookService bookService)
        {
            this.authorService = authorService;
            this.genreService = genreService;
            this.bookService = bookService;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {

            return View(await bookService.GetAllAsync(x => !x.IsDeleted , "BookImages" , "Genre" , "Author"));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await genreService.GetAllAsync(x => !x.IsDeleted);
            ViewBag.Authors = await authorService.GetAllAsync(x => !x.IsDeleted);
            return View();
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateVM vm)
        {
            ViewBag.Genres = await genreService.GetAllAsync(x => !x.IsDeleted);
            ViewBag.Authors = await authorService.GetAllAsync(x => !x.IsDeleted);

            if (!ModelState.IsValid) return View(vm);

            try
            {
                await bookService.CreateAsync(vm);
            }
            catch(EntityNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(vm);
            }
            catch (FileValidationException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                await bookService.DeleteAsync(id);
            }
            catch (IdIsNotValid)
            {
                return View("Id is not valid");
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Genres = await genreService.GetAllAsync(x => !x.IsDeleted);
            ViewBag.Authors = await authorService.GetAllAsync(x => !x.IsDeleted);

            Book data = null;

            try
            {
                data = await bookService.GetByExpressionAsync(x => x.Id == id, "BookImages", "Author", "Genre");
            }
            catch (EntityNotFoundException)
            {
                return View("Error");
            }

            BookUpdateVM bookVM = new()
            {
                Title = data.Title,
                Desc = data.Desc,
                StockCount = data.StockCount,
                AuthorId = data.AuthorId,
                GenreId = data.GenreId,
                IsAvailable = data.IsAvalible,
                Discount = data.Discount,
                CostPrice = data.CostPrice,
                SalePrice = data.SalePrice,
                ProductCode = data.ProductCode,
                BookImages = data.BookImages
            };



            return View(bookVM);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BookUpdateVM bookVM)
        {
            ViewBag.Genres = await genreService.GetAllAsync(x => !x.IsDeleted);
            ViewBag.Authors = await authorService.GetAllAsync(x => !x.IsDeleted);
            Book data = null;
            try
            {
                data = await bookService.GetByExpressionAsync(x => x.Id == id, "BookImages", "Author", "Genre") ?? throw new EntityNotFoundException();
            }
            catch (Exception)
            {
                return View("Error");
            }
            bookVM.BookImages = data.BookImages;

            if (id < 1 || id is null)
            {
                return View("Error");
            }

            if (!ModelState.IsValid)
            {
                return View(bookVM);
            }


            try
            {
                await bookService.UpdateAsync(id, bookVM);
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(bookVM);
            }
            catch (FileValidationException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(bookVM);
            }
            catch (IdIsNotValid ex)
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
    
