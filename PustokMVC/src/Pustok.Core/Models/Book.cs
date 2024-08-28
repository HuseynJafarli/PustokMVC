namespace Pustok.Core.Models
{
    public class Book: BaseEntity
    {
        public int AuthorId { get; set; }
        public int GenreId { get; set; }

        public string Title { get; set; }
        public string Desc { get; set; }
        public string ProductCode { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public int StockCount { get; set; }
        public int Discount { get; set; }
        public bool IsAvalible { get; set; }
        public Genre Genre { get; set; }
        public Author Author { get; set; }
        public List<BookImage> BookImages { get; set; }
    }
}
