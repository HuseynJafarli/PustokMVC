using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;

namespace Pustok.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _genreService.GetAllAsync(null));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Create(GenreCreateViewModel genreVM)
        {
            if (!ModelState.IsValid)
            {
                return View(genreVM);
            }

            try
            {
                await _genreService.CreateAsync(genreVM);
            }
            catch (GenreAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(genreVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(genreVM);
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var data = await _genreService.GetByIdAsync(id) ?? throw new NullReferenceException();
            GenreUpdateViewModel genreVM = new GenreUpdateViewModel()
            {
                Name = data.Name,
            };
            return View(genreVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, GenreUpdateViewModel genreVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _genreService.UpdateAsync(id, genreVM);
            }
            catch(GenreAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
