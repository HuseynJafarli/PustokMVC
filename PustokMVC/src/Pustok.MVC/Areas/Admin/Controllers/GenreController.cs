using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            return View(await _genreService.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateViewModel genreVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            await _genreService.CreateAsync(genreVM);

            return RedirectToAction(nameof(Index));
        }

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
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
