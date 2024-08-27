using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Services.Interfaces;
using System.Diagnostics;

namespace Pustok.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenreService _genreService;

        public HomeController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();

            return View(genres);
        }

    }
}
