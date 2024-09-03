using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;

namespace Pustok.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly ISlideService slideService;

        public SlideController(ISlideService slideService)
        {
            this.slideService = slideService;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await slideService.GetAllAsync());
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Create(SlideCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            try
            {
                await slideService.CreateAsync(vm);
            }
            catch(FileValidationException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                await slideService.DeleteAsync(id);
            }
            catch (IdIsNotValid)
            {
                return View("Id is not valid");
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var data = await slideService.GetByIdAsync(id) ?? throw new NullReferenceException();
            SlideUpdateViewModel slideVM = new SlideUpdateViewModel()
            {
                Title = data.Title,
                Description = data.Description,
                Image = data.Image

            };

            return View(slideVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, SlideUpdateViewModel slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View(slideVM);
            }
            await slideService.UpdateAsync(id, slideVM);
            return RedirectToAction(nameof(Index));
        }
    }
}
