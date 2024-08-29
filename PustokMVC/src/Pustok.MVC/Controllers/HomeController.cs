using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Services.Interfaces;
using System.Diagnostics;

namespace Pustok.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IBookService bookService;

        public HomeController(IGenreService genreService, IBookService bookService)
        {
            _genreService = genreService;
            this.bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await bookService.GetAllAsync(x => !x.IsDeleted , "BookImages" , "Author" , "Genre"));
        }

    }
}
