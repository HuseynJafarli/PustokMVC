using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.MVC.ViewModels;
using System.Diagnostics;

namespace Pustok.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IBookService bookService;
        private readonly ISlideService slideService;

        public HomeController(IGenreService genreService, IBookService bookService, ISlideService slideService)
        {
            _genreService = genreService;
            this.bookService = bookService;
            this.slideService = slideService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredBooks = await bookService.GetAllAsync(x => !x.IsDeleted, "BookImages", "Author", "Genre");
            var newArrivals = await bookService.GetAllByOrderAsync(x => !x.IsDeleted, b => b.CreatedAt, "BookImages", "Author", "Genre");

            var mostExpensiveBooks = await bookService.GetAllByOrderAsync(x => !x.IsDeleted, b => b.SalePrice, "BookImages", "Author", "Genre");
            var slides = await slideService.GetAllAsync(); 

            var model = new HomeVM
            {
                FeaturedBooks = featuredBooks,
                NewArrivals = newArrivals,
                MostExpensiveBooks = mostExpensiveBooks,
                Slides = slides
            };

            return View(model);
        }


        public async Task<IActionResult> AddToBasket(int? bookId)
        {
            if (bookId < 1 || bookId is null)
            {
                return NotFound();
            }

            if(await bookService.IsExist(x => x.Id == bookId) == false)
            {
                return NotFound();
            }


            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            BasketItemVM basketItem = null;
            string existedItems = HttpContext.Request.Cookies["BasketItems"];
            if (existedItems != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemVM>>(existedItems);

                basketItem = basketItems.FirstOrDefault(x => x.BookId == bookId);
                if (basketItem != null)
                {
                    basketItem.Count++;
                    
                }
                else
                {
                    basketItem = new BasketItemVM
                    {
                        BookId = bookId,
                        Count = 1
                    };
                    basketItems.Add(basketItem);
                }
            }
            else
            {
                basketItem = new BasketItemVM
                {
                    BookId = bookId,
                    Count = 1
                };
                basketItems.Add(basketItem);
            }
           
            
            string basketItemsStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);

            return Ok();

        }


        public IActionResult GetBasketItems()
        {
            string data = HttpContext.Request.Cookies["BasketItems"];
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            if (data != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemVM>>(data);
            }


            return Ok(basketItems);
        }


        //public IActionResult SetCookie(int id)
        //{
        //    List<int> Ids = new List<int>();
        //    var existList = HttpContext.Request.Cookies["Ids"];
        //    if (existList != null)
        //    {
        //        Ids = JsonConvert.DeserializeObject<List<int>>(existList);
        //    }
        //    Ids.Add(id);

        //    existList = JsonConvert.SerializeObject(Ids);

        //    HttpContext.Response.Cookies.Append("Ids", existList);

        //    return Content("Added");
        //}
        //public IActionResult GetCookie()
        //{
        //    string data = HttpContext.Request.Cookies["Ids"];
        //    List<int> Ids = new List<int>();
        //    if (data != null)
        //    {
        //        Ids = JsonConvert.DeserializeObject<List<int>>(data);
        //    }


        //    return Ok(Ids);
        //}

    }
}
