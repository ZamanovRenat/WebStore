using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> UserManager, 
            SignInManager<User> SignInManager,
            ILogger<AccountController> Logger)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
            _logger = Logger;
        }
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) { return View(Model); }

            _logger.LogInformation("Регистрация пользователя {0}", Model.UserName);

            var user = new User {UserName = Model.UserName};
            var register_result = await _userManager.CreateAsync(user, Model.Password);

            if (register_result.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);

                await _userManager.AddToRoleAsync(user, Role.Users);

                _logger.LogInformation("Пользователю {0} назначена роль {1}",
                    user.UserName, Role.Users);

                await _signInManager.SignInAsync(user, false);

                _logger.LogInformation("Пользователь {0} автоматически вошёл в систему после регистрации", user.UserName);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in register_result.Errors)
                ModelState.AddModelError("", error.Description);
            _logger.LogInformation("Ошибка при регистрации пользователя {0 в систему: {1}", 
                Model.UserName,
                string.Join(", ", register_result.Errors.Select(err => err.Description)));


            return View(Model);
        }

        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel {ReturnUrl = ReturnUrl});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var login_result = await _signInManager.PasswordSignInAsync(
                model.UserName, 
                model.Password, 
                model.RememberMe,
#if DEBUG
                 false
#else 
                true
#endif
                );

            if (login_result.Succeeded)
            {
                //if (Url.IsLocalUrl(model.ReturnUrl))
                //{
                //    return Redirect(model.ReturnUrl);
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Неправильный пароль или имя пользователя");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
