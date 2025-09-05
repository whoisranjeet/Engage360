using EmployeePortal.Core.DTOs;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Core.Models;
using EmployeePortal.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
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
        public IActionResult SignIn()
        {
            var username = Request.Form["Username"].ToString();
            var password = Request.Form["Password"].ToString();
            var rememberMe = Request.Form["RememberMe"].ToString() == "on";

            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var dto = new UserDto()
                {
                    Username = username,
                    Password = password
                };

                if (_employeeService.UserSignIn(dto))
                {
                    LogInAsync(dto);
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

            if(!string.IsNullOrEmpty(emailAddress))
            {
                var result = await _employeeService.GetUserByEmailAsync(emailAddress);
                if(result != null)
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

                if (_employeeService.UserSignUp(empDto))
                {
                    string emailBody = string.Format("Your account has been set up in our system.\nLogin Details:\nUsername: {0}\nPassword: {1}", empDto.EmailAddress, generatedPassword);
                    _emailService.SendEmailUsingGmail(empDto.EmailAddress, "Your account has been setup. : Engage360", emailBody);

                    UserDto userDto = new()
                    {
                        Username = empDto.EmailAddress,
                        Password = empDto.Password
                    };
                    if (_employeeService.UserSignIn(userDto))
                    {
                        LogInAsync(userDto);
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
        public IActionResult GetEmployees(int page = 1, int pageSize = 10)
        {
            var employees = _employeeService.GetEmployeesPaged(page, pageSize);
            return Json(employees);
        }

        [HttpGet]
        [Route("Modify-Role")]
        public async Task<IActionResult> ModifyEmployeeRole()
        {
            var model = new RoleMappingDto
            {
                Roles = _employeeService.GetAllRoles(),
                Employees = _employeeService.GetAllEmployees(),
                Users = await _employeeService.GetAllUsers(),
                EmailAddress = string.Empty,
                RoleName = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [Route("Modify-Role")]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        [Route("Delete-Employee")]
        public IActionResult DeleteEmployee()
        {
            return View(_employeeService.GetAllEmployees());
        }

        [HttpPost]
        [Route("Delete-Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee([FromBody] RoleMappingDto data)
        {
            if (data.EmailAddress != string.Empty)
            {
                if (await _employeeService.RemoveEmployee(data.EmailAddress))
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
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        [Route("HandleGoogleSignInUpAsync")]
        public async Task<IActionResult> HandleGoogleSignInUpAsync()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            //var users = await _employeeService.GetAllUsers();
            //var user = users.FirstOrDefault(u => u.Username == email);
            var user = await _employeeService.GetUserByEmailAsync(email);

            if (user != null)
            {
                LogInAsync(user);
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
                        LogInAsync(userDto);
                        return RedirectToAction("Dashboard", "Dashboard", new { auth = "google", status = "success" });
                    }
                }
            }
            return RedirectToAction("Dashboard", "Dashboard");
        }

        public async void LogInAsync(UserDto userDto)
        {
            var users = await _employeeService.GetAllUsers();
            var user = users.FirstOrDefault(u => u.Username == userDto.Username);
            var role = user != null ? _employeeService.GetAllRoles().FirstOrDefault(role => role.Id == user.RoleId) : null;

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
