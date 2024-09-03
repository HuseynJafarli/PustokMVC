using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Core.Models;
using Pustok.MVC.Areas.Admin.ViewModels;

namespace Pustok.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }
            AppUser appUser = null;

            appUser = await _userManager.FindByNameAsync(vm.UserName);

            if (appUser is null)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, vm.Password, vm.IsPersistent, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }

            return RedirectToAction("index", "Home");
        }
    }
}
