using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using newProject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace newProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationClass> userManager;
        private readonly SignInManager<ApplicationClass> signInManager;

        public AccountController(UserManager<ApplicationClass> userManager,
            SignInManager<ApplicationClass> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index","home");
        }
        // GET: /<controller>/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmail(string Email)
        {
             var user = await userManager.FindByEmailAsync(Email);
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"{Email} already registered");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationClass {
                    UserName = model.Email,
                    Email = model.Email,
                    city = model.City
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Listusers", "Administration");
                    }
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }





        
        // GET: /<controller>/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false) ;
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl)&&Url.IsLocalUrl(returnUrl ))
                        return Redirect(returnUrl);
                    else 
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Invalid Login");
                
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
