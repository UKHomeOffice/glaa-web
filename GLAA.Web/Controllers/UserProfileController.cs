using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IMapper mapper;
        private readonly UserManager<GLAAUser> userManager;
        private readonly SignInManager<GLAAUser> signInManager;

        public UserProfileController(IMapper mp, UserManager<GLAAUser> um, SignInManager<GLAAUser> sm)
        {
            mapper = mp;
            userManager = um;
            signInManager = sm;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var login = User.Identity.Name;
            var user = userManager.FindByEmailAsync(login).GetAwaiter().GetResult();
            var model = mapper.Map<AdminUserViewModel>(user) as UserViewModel;
            return View(model);
        }

        [HttpGet]
        public IActionResult EditName()
        {
            var login = User.Identity.Name;
            var user = userManager.FindByEmailAsync(login).GetAwaiter().GetResult();
            var model = new EditNameViewModel
            {
                Current = user.FullName
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditName(EditNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["doOverride"] = true;
                return View(model);
            }

            var login = User.Identity.Name;
            var user = userManager.FindByEmailAsync(login).GetAwaiter().GetResult();
            user.FullName = model.New;

            userManager.UpdateAsync(user).GetAwaiter().GetResult();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditEmail()
        {
            var login = User.Identity.Name;
            var user = userManager.FindByEmailAsync(login).GetAwaiter().GetResult();
            var model = new EditEmailViewModel
            {
                Current = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditEmail(EditEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["doOverride"] = true;
                return View(model);
            }

            if (userManager.FindByEmailAsync(model.New).GetAwaiter().GetResult() != null)
            {
                ViewData["doOverride"] = true;
                // TODO: A better error message?
                ModelState.AddModelError("legend_Email", "That email address is already in use in this service.");
                return View(model);
            }

            var login = User.Identity.Name;
            var user = userManager.FindByEmailAsync(login).GetAwaiter().GetResult();
            user.Email = model.New;
            user.UserName = model.New;

            userManager.UpdateAsync(user).GetAwaiter().GetResult();

            signInManager.SignOutAsync().GetAwaiter().GetResult();

            return RedirectToAction("Index", "Home");
        }
    }
}
