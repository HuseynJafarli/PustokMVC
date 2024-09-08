using Pustok.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Core.Models
{
    public class Order : BaseEntity
    {
        public string Fullname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Note { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? AppUserId { get; set; }
        public double TotalPrice { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
