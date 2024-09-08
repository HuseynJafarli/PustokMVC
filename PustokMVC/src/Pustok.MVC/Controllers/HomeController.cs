using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.Core.Models;
using Pustok.Data.DAL;
using Pustok.MVC.ViewModels;
using System.Diagnostics;

namespace Pustok.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IBookService bookService;
        private readonly ISlideService slideService;
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext appDbContext;

        public HomeController(IGenreService genreService, IBookService bookService, ISlideService slideService, UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _genreService = genreService;
            this.bookService = bookService;
            this.slideService = slideService;
            this.userManager = userManager;
            this.appDbContext = appDbContext;
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

            if (!await bookService.IsExist(x => x.Id == bookId))
            {
                return NotFound();
            }

            AppUser appUser = null;
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            BasketItemVM basketItem = null;
            BasketItem userBasketItem = null;
            string existedItems = HttpContext.Request.Cookies["BasketItems"];


            if (HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }

            if (appUser == null)
            {
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

            }
            else
            {
                userBasketItem = await appDbContext.BasketItems.FirstOrDefaultAsync(x => x.BookId == bookId && x.AppUserId == appUser.Id);
                if (userBasketItem != null)
                {
                    userBasketItem.Count++;
                }
                else
                {
                    userBasketItem = new BasketItem()
                    {
                        AppUserId = appUser.Id,
                        BookId = bookId,
                        Count = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false

                    };
                await appDbContext.BasketItems.AddAsync(userBasketItem);
                }

            }
            var result  = await appDbContext.SaveChangesAsync();
            return Ok();

        }


        public async Task<IActionResult> GetBasketItems()
        {
            AppUser appUser = null;
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            if (appUser == null)
            {
                string data = HttpContext.Request.Cookies["BasketItems"];
               
                if (data != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemVM>>(data);
                }
            }
            else
            {
                List<BasketItem> userBasketItems = new List<BasketItem>();
                userBasketItems = await appDbContext.BasketItems.Where(x => x.AppUserId == appUser.Id).ToListAsync();

                basketItems = userBasketItems.Select(x => new BasketItemVM()
                {
                    BookId = x.BookId,
                    Count = x.Count,
                }).ToList();
            }
            


            return Ok(basketItems);
        }

        public IActionResult BasketUpdate()
        {
            return ViewComponent("Basket");
        }
    }
}
