using Authentication2.Models;
using Authentication2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication2.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signManager;
        private readonly UserManager<AppUser> userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
           this.signManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel payload)
        {
            if (ModelState.IsValid)
            {
                var res = await signManager.PasswordSignInAsync(payload.Username!, payload.Password!, payload.RememberMe, false);
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); 
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(payload);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel payload)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Name = payload.Name,
                    UserName = payload.Email,
                    Email = payload.Email,
                    Address = payload.Address
                };

                var res = await userManager.CreateAsync(user, payload.Password!);
                if (res.Succeeded)
                {
                    await signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
