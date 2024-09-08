namespace Pustok.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public int? BookId { get; set; }
        public string AppUserId { get; set; }
        public int Count { get; set; }
        public Book Book { get; set; }
        public AppUser User { get; set; }
    }
}
