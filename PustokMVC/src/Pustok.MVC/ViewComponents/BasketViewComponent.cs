using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pustok.Business.Services.Interfaces;
using Pustok.MVC.ViewModels;

namespace Pustok.MVC.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly IBookService _bookService;

        public BasketViewComponent(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string data = HttpContext.Request.Cookies["BasketItems"];
            List<ItemInCartVM> basketItems = new List<ItemInCartVM>();

            if (data != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<ItemInCartVM>>(data);

                foreach (var item in basketItems)
                {
                    var book = await _bookService.GetByExpressionAsync(x => x.Id == item.BookId , "BookImages");
                    item.Book = book;
                }
            }

            return View(basketItems);
        }
    }

}
