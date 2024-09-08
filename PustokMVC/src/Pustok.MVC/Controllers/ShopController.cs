using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Business.ExternalServices.Interfaces;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.Core.Models;
using Pustok.Data.DAL;
using Pustok.MVC.ViewModels;
using System.Net;
using System.Net.Mail;

namespace Pustok.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly IBookService bookService;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;

        public ShopController(AppDbContext appDbContext, IBookService bookService, UserManager<AppUser> userManager, IEmailService emailService)
        {
            this.appDbContext = appDbContext;
            this.bookService = bookService;
            this.userManager = userManager;
            this.emailService = emailService;
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
            string basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderVM vm)
        {

            AppUser appUser = null;

            List<CheckoutVM> checkoutVMs = new List<CheckoutVM>();
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            string basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
            List<BasketItem> userBasketItems = [];


            if (HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }


            Order order = new Order()
            {
                Fullname = vm.Fullname,
                Phone = vm.Phone,
                EmailAddress = vm.EmailAddress,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Note = vm.Note,
                ZipCode = vm.ZipCode,
                Country = vm.Country,
                IsDeleted = false,
                Address = vm.Address,
                AppUserId = appUser?.Id,
                OrderItems = [],
                //OrderStatus = OrderStatus.Pending,
                City = vm.City,
                TotalPrice = 0
            };




            if (appUser is null)
            {
                if (basketItemsStr is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemVM>>(basketItemsStr);


                    foreach (var item in basketItems)
                    {

                        Book book = await bookService.GetByIdAsync(item.BookId);

                        OrderItem orderItem = new()
                        {

                            Title = book.Title,
                            CostPrice = book.CostPrice,
                            BookId = book.Id,
                            SalePrice = book.SalePrice,
                            DiscountPercent = book.Discount,
                            Count = item.Count,
                            Order = order,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsDeleted = false,
                        };

                        order.TotalPrice += Math.Round(orderItem.Count * (orderItem.SalePrice - (orderItem.DiscountPercent * orderItem.SalePrice / 100)), 2);
                        order.OrderItems.Add(orderItem);

                    }
                    HttpContext.Response.Cookies.Delete("BasketItems");
                }
            }
            else
            {
                userBasketItems = await appDbContext.BasketItems.Include(x => x.Book).Where(x => x.AppUserId == appUser.Id && x.IsDeleted == false).ToListAsync();

                foreach (var item in userBasketItems)
                {
                    Book book = await bookService.GetByIdAsync(item.BookId);

                    OrderItem orderItem = new()
                    {

                        Title = book.Title,
                        CostPrice = book.CostPrice,
                        BookId = book.Id,
                        SalePrice = book.SalePrice,
                        DiscountPercent = book.Discount,
                        Count = item.Count,
                        Order = order,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    order.TotalPrice += Math.Round(orderItem.Count * (orderItem.SalePrice - (orderItem.DiscountPercent * orderItem.SalePrice / 100)), 2);
                    order.OrderItems.Add(orderItem);
                }
            }

            foreach (BasketItem bi in userBasketItems)
            {
                bi.IsDeleted = true;
            }

            await appDbContext.Orders.AddAsync(order);
            await appDbContext.SaveChangesAsync();

            await emailService.SendMailAsync(vm.EmailAddress, "Pustok", vm.Fullname, "Your order has been received");

            return RedirectToAction("Index", "Home");
        }
    }
}

