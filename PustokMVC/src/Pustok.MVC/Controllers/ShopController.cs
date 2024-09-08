using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.Core.Models;
using Pustok.Data.DAL;
using Pustok.MVC.ViewModels;

namespace Pustok.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly IBookService bookService;
        private readonly UserManager<AppUser> userManager;

        public ShopController(AppDbContext appDbContext, IBookService bookService, UserManager<AppUser> userManager)
        {
            this.appDbContext = appDbContext;
            this.bookService = bookService;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            List<CheckoutVM> checkoutVMs = new List<CheckoutVM>();



            AppUser appUser = null;
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            string basketItemsStr = HttpContext.Request.Cookies["Items"];
            List<BasketItem> userBasketItems = [];

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }

            if (appUser is null)
            {
                if (basketItemsStr is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemVM>>(basketItemsStr);
                    checkoutVMs = basketItems.Select(bi => new CheckoutVM()
                    {
                        Book = bookService.GetByIdAsync(bi.BookId).Result,
                        Count = bi.Count
                    }).ToList();

                }
            }
            else
            {
                userBasketItems = await appDbContext.BasketItems.Include(x => x.Book).Where(x => x.AppUserId == appUser.Id && x.IsDeleted == false).ToListAsync();
                checkoutVMs = userBasketItems.Select(ubi => new CheckoutVM { Book = ubi.Book, Count = ubi.Count }).ToList();
            }

            OrderVM orderVM = new OrderVM()
            {
                CheckoutVMs = checkoutVMs,
                EmailAddress = appUser?.Email,
                Fullname = appUser?.Fullname,
                Phone = appUser?.PhoneNumber
            };

            return View(orderVM);
        }
    }
}
