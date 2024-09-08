using Pustok.Core.Models;

namespace Pustok.MVC.ViewModels
{
    public class CheckoutVM
    {
        public Book? Book { get; set; }

        public int Count { get; set; }
    }
}