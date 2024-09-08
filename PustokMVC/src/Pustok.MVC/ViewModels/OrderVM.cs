using System.ComponentModel.DataAnnotations;

namespace Pustok.MVC.ViewModels
{
    public class OrderVM
    {
        public List<CheckoutVM> CheckoutVMs { get; set; }

        public string Fullname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Note { get; set; }
    }
}
