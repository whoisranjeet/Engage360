using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult UserSignIn(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                if (_employeeService.UserSignIn(userDto))
                {
                    LogUserIn(userDto);
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                ModelState.AddModelError("Error: ", "Invalid username or password.");
            }
            return View(userDto);
        }

        public async Task<IActionResult> UserSignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("UserSignIn");
        }

        [Route("SignUp")]
        [AllowAnonymous]
        public IActionResult UserSignUp()
        {
            return View();
        }

        [Route("SignUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult UserSignUp(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                if (_employeeService.UserSignUp(employeeDto))
                {
                    UserDto userDto = new()
                    {
                        Username = employeeDto.EmailAddress,
                        Password = employeeDto.Password
                    };
                    if (_employeeService.UserSignIn(userDto))
                    {
                        LogUserIn(userDto);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            }
            return View(employeeDto);
        }

        public async void LogUserIn(UserDto userDto)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userDto.Username)
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
        }

        [Route("New Employee")]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [Route("New Employee")]
        [HttpPost]
        public IActionResult AddEmployee(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                _employeeService.AddEmployee(employeeDto);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View(employeeDto);
        }
    }
}
