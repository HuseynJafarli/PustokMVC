using Microsoft.AspNetCore.Http;
using Pustok.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Pustok.Business.ViewModels
{
    public class BookUpdateVM
    {
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(450)]
        public string Desc { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public int Discount { get; set; }
        public bool IsAvailable { get; set; }
        public int StockCount { get; set; }
        [StringLength(10)]
        public string ProductCode { get; set; }

        public IFormFile PosterImage { get; set; }
        public IFormFile HoverImage { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public List<int>? BookImageIds { get; set; }
        public List<BookImage>? BookImages { get; set; }
    }
}
