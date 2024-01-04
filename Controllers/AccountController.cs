using CustomIdentityAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentityAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RemeberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(loginModel);
            }
            return View(loginModel);

        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                User user = new User { 
                    Name=model.Name,
                    Email=model.Email,
                    UserName=model.Email,
                    Address=model.Address
                };
                var result = await _userManager.CreateAsync(user,model.Password);
                if(result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("User");

                    if (!roleExists)
                    {
                        // Create the "User" role
                        var role = new IdentityRole("User");
                        await _roleManager.CreateAsync(role);
                    }

                    // Assign the user to the "User" role
                    await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index","Home");
                }
                foreach(var errors in result.Errors)
                {
                    ModelState.AddModelError("",errors.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
