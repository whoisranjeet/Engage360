using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EmployeePortal.Controllers
{
    [Authorize]
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

        [HttpPost]
        [AllowAnonymous]
        [Route("SignInUsingUsernamePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn()
        {
            var username = Request.Form["Username"].ToString();
            var password = Request.Form["Password"].ToString();
            var rememberMe = Request.Form["RememberMe"].ToString() == "on";

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var dto = new UserDto()
                {
                    Username = username,
                    Password = password
                };

                if (await _employeeService.UserSignInAsync(dto))
                {
                    await LogInAsync(dto);
                    return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Dashboard") });
                }
            }

            return Json(new { success = false, message = "Invalid username or password." });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SignUpUsingForm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp()
        {
            var firstName = Request.Form["firstName"].ToString();
            var lastName = Request.Form["lastName"].ToString();
            var emailAddress = Request.Form["emailAddress"].ToString();
            var mobileNumber = Request.Form["mobileNumber"].ToString();

            if (!string.IsNullOrEmpty(emailAddress))
            {
                var result = await _employeeService.GetUserByEmailAsync(emailAddress);
                if (result != null)
                {
                    return Json(new { success = false, message = "No need to sign up twice, we've got you covered! Just sign in." });
                }
            }

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(emailAddress))
            {
                var generatedPassword = GeneratePassword(lastName);

                var empDto = new EmployeeDto()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    MobileNumber = mobileNumber,
                    Password = generatedPassword
                };

                if (await _employeeService.UserSignUpAsync(empDto))
                {
                    string emailBody = string.Format("Your account has been set up in our system.\nLogin Details:\nUsername: {0}\nPassword: {1}", empDto.EmailAddress, generatedPassword);
                    _emailService.SendEmailUsingGmail(empDto.EmailAddress, "Your account has been setup. : Engage360", emailBody);

                    UserDto userDto = new()
                    {
                        Username = empDto.EmailAddress,
                        Password = empDto.Password
                    };
                    if (await _employeeService.UserSignInAsync(userDto))
                    {
                        await LogInAsync(userDto);
                        return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Dashboard") });
                    }
                }
            }

            return Json(new { success = false, message = "Something went wrong." });
        }

        [HttpGet]
        [Route("New-Employee")]
        public IActionResult AddEmployee()
        {
            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View();
        }

        [HttpPost]
        [Route("New-Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeeDto employeeDto, IFormFile imageUpload)
        {
            if (imageUpload != null && imageUpload.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageUpload.CopyToAsync(memoryStream);
                    employeeDto.ProfilePicture = memoryStream.ToArray();
                }
            }

            var generatedPassword = GeneratePassword(employeeDto.LastName);
            employeeDto.Password = generatedPassword;

            ModelState.Remove(nameof(employeeDto.Password));
            ModelState.Remove(nameof(employeeDto.ProfilePicture));

            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employeeDto);
                string emailBody = string.Format("Your account has been set up in our system.\nLogin Details:\nUsername: {0}\nPassword: {1}", employeeDto.EmailAddress, generatedPassword);
                _emailService.SendEmailUsingGmail(employeeDto.EmailAddress, "Your account has been setup. : Engage360", emailBody);

                TempData["SuccessMessage"] = "Employee Details Added Successfully !!!";
                return RedirectToAction("Dashboard", "Dashboard");
            }

            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View(employeeDto);
        }

        //[HttpGet]
        //[Route("My-Organization")]
        //public IActionResult GetAllEmployee()
        //{
        //    var employees = _employeeService.GetAllEmployees();
        //    return View(employees);
        //}

        // Loads only the view
        [HttpGet]
        [Route("My-Organization")]
        public IActionResult GetAllEmployee()
        {
            return View();
        }

        // API endpoint for infinite scroll (AJAX call)
        [HttpGet]
        [Route("api/employees")]
        public async Task<IActionResult> GetEmployees(int page = 1, int pageSize = 10)
        {
            var employees = await _employeeService.GetEmployeesPagedAsync(page, pageSize);
            return Json(employees);
        }

        [HttpGet]
        [Route("Modify-Role")]
        public async Task<IActionResult> ModifyEmployeeRole()
        {
            var model = new RoleMappingDto
            {
                Roles = await _employeeService.GetAllRolesAsync(),
                Employees = await _employeeService.GetAllEmployeesAsync(),
                Users = await _employeeService.GetAllUsersAsync(),
                EmailAddress = string.Empty,
                RoleName = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [Route("Modify-Role")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyEmployeeRole([FromBody] RoleMappingDto data)
        {
            if (data.EmailAddress != string.Empty && data.RoleName != string.Empty)
            {
                if (await _employeeService.ModifyEmployeeRoleAsync(data.EmailAddress, data.RoleName))
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpGet]
        [Route("Delete-Employee")]
        public async Task<IActionResult> DeleteEmployee()
        {
            return View(await _employeeService.GetAllEmployeesAsync());
        }

        [HttpPost]
        [Route("Delete-Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee([FromBody] RoleMappingDto data)
        {
            if (data.EmailAddress != string.Empty)
            {
                if (await _employeeService.RemoveEmployeeAsync(data.EmailAddress))
                {
                    if (data.EmailAddress == User.Identity.Name)
                    {
                        await SignOut();
                    }
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpGet]
        [Route("Update-Employee")]
        public async Task<IActionResult> UpdateEmployee()
        {
            var model = new UpdateEmployeeViewModel
            {
                Employees = await _employeeService.GetAllEmployeesAsync(),
                SelectedEmployee = new EmployeeDto()
            };

            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View(model);
        }

        [HttpPost]
        [Route("Update-Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeViewModel model, IFormFile imageUpload, string emailAddress)
        {
            model.SelectedEmployee.EmailAddress = emailAddress;

            if (imageUpload != null && imageUpload.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageUpload.CopyToAsync(memoryStream);
                    model.SelectedEmployee.ProfilePicture = memoryStream.ToArray();
                }
            }

            if (await _employeeService.UpdateEmployeeDetailsAsync(model.SelectedEmployee, emailAddress))
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            var viewModel = new UpdateEmployeeViewModel
            {
                Employees = await _employeeService.GetAllEmployeesAsync(),
                SelectedEmployee = model.SelectedEmployee
            };

            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());
            ViewBag.Genders = new SelectList(_portalHelper.GetAllGender());

            return View(viewModel);
        }

        [HttpGet]
        [Route("HandleGoogleSignInUpAsync")]
        public async Task<IActionResult> HandleGoogleSignInUpAsync()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _employeeService.GetUserByEmailAsync(email);

            if (user != null)
            {
                await LogInAsync(user);
                return RedirectToAction("Dashboard", "Dashboard", new { auth = "google", status = "success" });
            }
            else
            {
                var firstName = User.FindFirst(ClaimTypes.GivenName)?.Value;
                var lastName = User.FindFirst(ClaimTypes.Surname)?.Value;
                var generatedPassword = GeneratePassword(lastName);

                var employeeDto = new EmployeeDto()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = email,
                    Password = generatedPassword
                };

                if (await _employeeService.UserSignUpAsync(employeeDto))
                {
                    string emailBody = string.Format("Your account has been set up in our system.\nLogin Details:\nUsername: {0}\nPassword: {1}", employeeDto.EmailAddress, generatedPassword);
                    _emailService.SendEmailUsingGmail(employeeDto.EmailAddress, "Your account has been setup. : Engage360", emailBody);

                    UserDto userDto = new()
                    {
                        Username = employeeDto.EmailAddress,
                        Password = employeeDto.Password
                    };
                    if (await _employeeService.UserSignInAsync(userDto))
                    {
                        await LogInAsync(userDto);
                        return RedirectToAction("Dashboard", "Dashboard", new { auth = "google", status = "success" });
                    }
                }
            }
            return RedirectToAction("Dashboard", "Dashboard");
        }

        public async Task LogInAsync(UserDto userDto)
        {
            var users = await _employeeService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Username == userDto.Username);
            var roles = await _employeeService.GetAllRolesAsync();
            var role = user != null && roles != null ? roles.FirstOrDefault(role => role.Id == user.RoleId) : null;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, role.RoleName),
                new(ClaimTypes.Email, user.Username)
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

        [HttpGet]
        [Route("SignOut")]
        public async new Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Dashboard", "Dashboard");
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
    }
}
