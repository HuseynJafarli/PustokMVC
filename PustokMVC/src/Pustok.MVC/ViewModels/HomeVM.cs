using Pustok.Core.Models;

namespace Pustok.MVC.ViewModels
{
    public class HomeVM
    {
        public ICollection<Book> FeaturedBooks { get; set; }
        public ICollection<Book> NewArrivals { get; set; }
        public ICollection<Book> MostExpensiveBooks { get; set; }
        public ICollection<Slide> Slides { get; set; }
    }
}
