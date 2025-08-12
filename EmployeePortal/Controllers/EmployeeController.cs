using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeePortal.Services.Services;
using EmployeePortal.ViewModel;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly IPortalHelper _portalHelper;

        public EmployeeController(IEmployeeService employeeService, IEmailService emailService, IPortalHelper portalHelper)
        {
            _employeeService = employeeService;
            _emailService = emailService;
            _portalHelper = portalHelper;
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
                var generatedPassword = GeneratePassword(employeeDto.LastName);
                employeeDto.Password = generatedPassword;

                if (_employeeService.UserSignUp(employeeDto))
                {
                    string emailBody = string.Format("Your account has been set up in our system.\nLogin Details:\nUsername: {0}\nPassword: {1}", employeeDto.EmailAddress, generatedPassword);
                    _emailService.SendEmailUsingGmail(employeeDto.EmailAddress, "Your account has been setup. : Engage360", emailBody);

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
            var user = _employeeService.GetAllUsers().FirstOrDefault(user => user.Username == userDto.Username);
            var role = user != null ? _employeeService.GetAllRoles().FirstOrDefault(role => role.Id == user.RoleId) : null;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, role.RoleName)
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

        [Route("New-Employee")]
        public IActionResult AddEmployee()
        {
            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View();
        }

        [Route("New-Employee")]
        [HttpPost]
        public IActionResult AddEmployee(EmployeeDto employeeDto, IFormFile imageUpload)
        {
            if (imageUpload != null && imageUpload.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageUpload.CopyToAsync(memoryStream);
                    employeeDto.ProfilePicture = memoryStream.ToArray();
                }
            }

            var generatedPassword = GeneratePassword(employeeDto.LastName);
            employeeDto.Password = generatedPassword;

            ModelState.Remove(nameof(employeeDto.Password));
            ModelState.Remove(nameof(employeeDto.ProfilePicture));

            if (ModelState.IsValid)
            {
                _employeeService.AddEmployee(employeeDto);
                string emailBody = string.Format("Your account has been set up in our system.\nLogin Details:\nUsername: {0}\nPassword: {1}", employeeDto.EmailAddress, generatedPassword);
                _emailService.SendEmailUsingGmail(employeeDto.EmailAddress, "Your account has been setup. : Engage360", emailBody);

                TempData["SuccessMessage"] = "Employee Details Added Successfully !!!";
                return RedirectToAction("Dashboard", "Dashboard");
            }

            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View(employeeDto);
        }

        private string GeneratePassword(string lastName)
        {
            if (string.IsNullOrEmpty(lastName) || lastName.Length < 3)
            {
                throw new ArgumentException("LastName must have at least 3 characters for password generation.");
            }

            var namePart = char.ToUpper(lastName[0]) + lastName.Substring(1, 2).ToLower();
            var monthPart = DateTime.Now.ToString("MMM");
            var yearPart = DateTime.Now.ToString("yy");

            return $"{namePart}@{monthPart}{yearPart}";
        }

        [Route("My-Organization")]
        public IActionResult GetAllEmployee()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }

        [Route("Modify-Role")]
        public IActionResult ModifyEmployeeRole()
        {
            var model = new RoleMappingDto
            {
                Roles = _employeeService.GetAllRoles(),
                Employees = _employeeService.GetAllEmployees(),
                Users = _employeeService.GetAllUsers(),
                EmailAddress = string.Empty,
                RoleName = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [Route("Modify-Role")]
        public IActionResult ModifyEmployeeRole([FromBody] RoleMappingDto data)
        {
            if (data.EmailAddress != string.Empty && data.RoleName != string.Empty)
            {
                if (_employeeService.ModifyEmployeeRole(data.EmailAddress, data.RoleName))
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [Route("Delete-Employee")]
        public IActionResult DeleteEmployee()
        {
            return View(_employeeService.GetAllEmployees());
        }

        [HttpPost]
        [Route("Delete-Employee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] RoleMappingDto data)
        {
            if (data.EmailAddress != string.Empty)
            {
                if (await _employeeService.RemoveEmployee(data.EmailAddress))
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }


        [Route("Update-Employee")]
        public IActionResult UpdateEmployee()
        {
            var model = new UpdateEmployeeViewModel
            {
                Employees = _employeeService.GetAllEmployees(),
                SelectedEmployee = new EmployeeDto()
            };

            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View(model);
        }

        [HttpPost]
        [Route("Update-Employee")]
        public IActionResult UpdateEmployee(UpdateEmployeeViewModel model, IFormFile imageUpload, string emailAddress)
        {
            model.SelectedEmployee.EmailAddress = emailAddress;

            if (imageUpload != null && imageUpload.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageUpload.CopyToAsync(memoryStream);
                    model.SelectedEmployee.ProfilePicture = memoryStream.ToArray();
                }
            }

            if (_employeeService.UpdateEmployeeDetails(model.SelectedEmployee, emailAddress))
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            var viewModel = new UpdateEmployeeViewModel
            {
                Employees = _employeeService.GetAllEmployees(),
                SelectedEmployee = model.SelectedEmployee
            };

            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View(viewModel);
        }
    }
}
