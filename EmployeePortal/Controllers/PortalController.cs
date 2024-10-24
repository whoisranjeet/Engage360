using EmployeePortal.Data;
using EmployeePortal.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EmployeePortal.Controllers
{
    [Authorize]
    public class PortalController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly DBOperations _dbOperations;

        public PortalController(AppDbContext context, DBOperations dBOperations)
        {
            _dbContext = context;
            _dbOperations = dBOperations;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await LogUserIn(model.Username, model.Password))
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                ModelState.AddModelError("Error: ", "Invalid username or password.");
            }
            return View(model);
        }

        private async Task<bool> LogUserIn(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, username),
                        new(ClaimTypes.Role, "Admin")
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            return false;
        }

        new public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }

        [Route("SignUp")]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("SignUp")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpAsync(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dbOperations.AddEmployee(model))
                {
                    if (await LogUserIn(model.EmailAddress, model.Password))
                    {
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            }
            return View(model);
        }
    }
}
