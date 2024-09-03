using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Core.Models;

namespace Pustok.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser user = new AppUser()
        //    {
        //        Fullname = "Huseyn Jafarli",
        //        UserName = "hjafarli",
        //        Email = "huseyncfrli6@gmail.com"
        //    };

        //    return Ok(await userManager.CreateAsync(user, "Salam_123"));
        //}

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role = new IdentityRole("SuperAdmin");
        //    IdentityRole role2 = new IdentityRole("Admin");
        //    IdentityRole role3 = new IdentityRole("Member");

        //   await roleManager.CreateAsync(role);
        //   await roleManager.CreateAsync(role2);
        //   await roleManager.CreateAsync(role3);

        //    return Ok();
        //}

        //public async Task<IActionResult> AddRole()
        //{
        //    AppUser appUser = await userManager.FindByNameAsync("hjafarli");

        //    return Ok(await userManager.AddToRoleAsync(appUser, "SuperAdmin"));
        //}
    }
}
