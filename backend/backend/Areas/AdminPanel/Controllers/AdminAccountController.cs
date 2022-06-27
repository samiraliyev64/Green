using backend.Areas.AdminPanel.Models;
using backend.Areas.AdminPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AdminAccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }
        public AdminAccountController(UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //SignIn (GET)
        public IActionResult SignIn()
        {
            return View();
        }
        //SignIn (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInVM user)
        {
            AppUser userDb = await _userManager.FindByEmailAsync(user.Email);
            if (userDb == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(user);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(userDb.UserName, user.Password, user.isPersistent, lockoutOnFailure: true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Please try a few minutes later.");
                return View(user);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(user);
            }
            if (!userDb.IsActivated)
            {
                ModelState.AddModelError("", "Please verify your account");
                return View(user);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
