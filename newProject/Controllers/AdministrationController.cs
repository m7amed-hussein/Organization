using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using newProject.Models;

namespace newProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController:Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationClass> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationClass> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.Name
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("listrole", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRole()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={id} is NOT FOUND";
                return View("notfound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach(var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={model.Id} is NOT FOUND";
                return View("notfound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("listrole", "Administration");
                }
                foreach( var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={roleId} is NOT FOUND";
                return View("notfound");
            }
            var model = new List<UserRoleViewModel>();

            foreach(var user in userManager.Users)
            {
                var UserViewModel = (new UserRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id
                });
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserViewModel.IsSelected = true;
                }
                else
                {
                    UserViewModel.IsSelected = false;
                }
                model.Add(UserViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> models, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={roleId} is NOT FOUND";
                return View("notfound");
            }
            for(int i =0;i<models.Count;i++)
            {
                IdentityResult result = null;
                var user = await userManager.FindByIdAsync(models[i].UserId);
                if(models[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result  = await userManager.AddToRoleAsync(user, role.Name);
                }else if (!models[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (models.Count - 1))
                        continue;
                    else
                    {
                        return RedirectToAction("EditRole",new { Id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }


        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id ={id} is NOT FOUND";
                return View("notfound");
            }
            var roles =  await userManager.GetRolesAsync(user);
            var claims = await userManager.GetClaimsAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                City = user.city,
                UserName = user.UserName,
                Claims = claims.Select(c => c.Value).ToList(),
                Roles = roles.ToList()
            };
            
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id ={model.Id} is NOT FOUND";
                return View("notfound");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.city = model.City;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("listusers", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id ={Id} is NOT FOUND";
                return View("notfound");
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("listusers", "Administration");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("listUsers");
        }
        [HttpPost]
        [Authorize(policy: "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id ={Id} is NOT FOUND";
                return View("notfound");
            }
            try
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("listrole", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("listrole");
            }catch(DbUpdateException)
            {
                ViewBag.ErrorTitle = $"{role.Name} is in use";
                ViewBag.ErrorMessage = $"{role.Name} can't be deleted because their is users in it, " +
                    $"if you want to delete it you must delete the users from the role first";
                return View("Error");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult>ManageClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with id ={userId} is NOT FOUND";
                return View("notfound");
            }
            var exsetingClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId
            };

            foreach(var claim in ClaimStore.Claims)
            {
                UserClaim userClaim = new UserClaim()
                {
                    ClaimType = claim.Type
                };

                if (exsetingClaims.Any(a => a.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                else
                {
                    userClaim.IsSelected = false;
                }

                model.Claims.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id ={model.UserId} is NOT FOUND";
                return View("notfound");
            }
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "can't remove existing user claims");
                return View(model);

            }
            result = await userManager.AddClaimsAsync(user, model.Claims.Where(c => c.IsSelected)
                .Select(c=>new Claim(c.ClaimType,c.ClaimType)));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "can't add existing user claims");
                return View(model);
            }
            return RedirectToAction("Edituser", new { Id = model.UserId });
        }
    }
}
