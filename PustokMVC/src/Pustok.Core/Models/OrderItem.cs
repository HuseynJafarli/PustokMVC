using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Core.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
