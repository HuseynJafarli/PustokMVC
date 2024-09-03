using System.ComponentModel.DataAnnotations;

namespace Pustok.MVC.Areas.Admin.ViewModels
{
    public class AdminLoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
