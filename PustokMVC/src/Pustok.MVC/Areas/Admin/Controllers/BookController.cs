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
        public async Task<IActionResult> Index()
        {

            return View(await bookService.GetAllAsync(x => !x.IsDeleted , "BookImages" , "Genre" , "Author"));
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
    }
}
