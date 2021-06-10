using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
        }
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) { return View(Model); }

            var user = new User {UserName = Model.UserName};
            var register_result = await _userManager.CreateAsync(user, Model.Password);

            if (register_result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in register_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(Model);
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
