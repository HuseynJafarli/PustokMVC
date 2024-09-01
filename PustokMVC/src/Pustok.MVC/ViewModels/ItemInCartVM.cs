using Pustok.Core.Models;

namespace Pustok.MVC.ViewModels
{
    public class ItemInCartVM
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int Count { get; set; }
    }
}
