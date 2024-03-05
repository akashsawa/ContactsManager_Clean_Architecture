using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CrudExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactsManager.UI.Controllers
{


    [Route("[controller]/[action]")]
    //[AllowAnonymous] // for authentication/authorization : means user without user account.
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //for user roles
        private readonly RoleManager<ApplicationRole> _roleManager;
        //for user roles

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

        [HttpGet]

        //CUSTOM AUTHORIZATION POLICIES
        [Authorize("NotAuthenticated")] // means when we hit the this register action method then the policy in the program.cs file gets executed.
        //CUSTOM AUTHORIZATION POLICIES

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //CUSTOM AUTHORIZATION POLICIES
        [Authorize("NotAuthenticated")] // means when we hit the this register action method then the policy in the program.cs file gets executed.
        //CUSTOM AUTHORIZATION POLICIES
        
        ////XSRF
        //[ValidateAntiForgeryToken]
        ////XSRF

        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            //check for validation errors
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser() { Email = registerDTO.Email, PhoneNumber = registerDTO.Phone, UserName = registerDTO.Email, PersonName = registerDTO.PersonName };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password); // usermanager allows to add ,delete, update the user data. here hashed password will be stored which is sha encrypted so original password cannot be seen by anyone.



            if (result.Succeeded)
            {
                //for user roles
                //check status of radio button
                if (registerDTO.UserType == Core.Enums.UserTypeOptions.Admin)
                {
                    //create admin role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }
                    //add the new user into admin role.
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString()); // for adding user with particular role in the table.
                }
                else
                {
                    //create admin role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.User.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString()); // for adding user with particular role in the table.
                }
                //for user roles

                //signin manager
                await _signInManager.SignInAsync(user, isPersistent: false); // ispersistant:true means cookie will remain in the browser means its a persistant cookie. here user is signed in  successfully. 
                                                                             //ispersistant:false means on closing the browser cookie will be deleted and user again will have to login int he system.
                                                                             //signin manager

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
            }
            return View(registerDTO);



        }

        [HttpGet]
        //CUSTOM AUTHORIZATION POLICIES
        [Authorize("NotAuthenticated")] // means when we hit the this register action method then the policy in the program.cs file gets executed.
        //CUSTOM AUTHORIZATION POLICIES
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //CUSTOM AUTHORIZATION POLICIES
        [Authorize("NotAuthenticated")] // means when we hit the this register action method then the policy in the program.cs file gets executed.
        //CUSTOM AUTHORIZATION POLICIES
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp =>
temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false); // means if user has entered wrong password then lockout that account , here we are not implementing lockout functionality.

            if (result.Succeeded)
            {
                //for Admin area:
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    if(await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index","Home", new { area="Admin"});
                    }
                }
                //for Admin area:

                //returnurl
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                //returnurl
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }

            ModelState.AddModelError("Login", "Invalid email or passwords");
            return View();
        }

        [HttpGet]
        //for custom authorization policy
        [Authorize] //means this action method should be authorized.
        //for custom authorization policy
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        //for remote validation
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        //for remote validation
    }
}
