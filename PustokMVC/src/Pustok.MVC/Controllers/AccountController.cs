using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;
using Pustok.Data.DAL;
using Pustok.MVC.ViewModels;

namespace Pustok.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, AppDbContext appDbContext, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
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
        public async Task<IActionResult> Login(MemberLoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }
            AppUser appUser = null;

            appUser = await _userManager.FindByNameAsync(vm.Username);

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

            return RedirectToAction("index", "home");
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = null;

            appUser = await _appDbContext.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == vm.Email.ToUpper());

            if (appUser is { })
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View();
            }

            appUser = await _userManager.FindByNameAsync(vm.Username);

            if (appUser is { })
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View();
            }

            appUser = new AppUser()
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username,
            };

            var result = await _userManager.CreateAsync(appUser, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }

            var member = await _userManager.FindByNameAsync(vm.Username);

            if (member is not null)
            {
                await _userManager.AddToRoleAsync(member, "Member");
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser appUser = null;
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            if (appUser == null)
            {
                return RedirectToAction("login");
            }
            else
            {
                List<Order> orders = await _appDbContext.Orders.Include(x => x.OrderItems).Where(x => x.AppUserId == appUser.Id && x.IsDeleted == false).ToListAsync();
                return View(orders);

            }
        }
    }
}
