using EmployeePortal.Core.Interfaces;
using EmployeePortal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Security.Claims;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace EmployeePortal.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<PayrollController> _logger;
        private readonly IPortalHelper _portalHelper;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ISalaryService _salaryService;

        public PayrollController(IPortalHelper portalHelper, IEmployeeService employeeService, ILogger<PayrollController> logger, ICompositeViewEngine viewEngine, ISalaryService salaryService)
        {
            _employeeService = employeeService;
            _logger = logger;
            _portalHelper = portalHelper;
            _viewEngine = viewEngine;
            _salaryService = salaryService;
        }

        [Route("Payroll")]
        public IActionResult Index()
        {
            var currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
            var duration = _salaryService.GetAvailableDuration(currentUser);

            ViewBag.Duration = new SelectList(duration);

            return View();
        }

        [Route("Generate Payroll")]
        public IActionResult GeneratePayroll()
        {
            ViewBag.Departments = new SelectList(_portalHelper.GetAllDepartment());

            return View();
        }

        [HttpPost]
        public JsonResult FilterEmployees(string department, DateTime? payPeriod)
        {
            var employees = _employeeService.GetAllEmployees();

            if (!string.IsNullOrEmpty(department))
                employees = employees.Where(e => e.Department == department).ToList();

            return Json(employees);
        }

        [HttpPost]
        public JsonResult GenerateSalaries([FromBody] GeneratePayrollViewModel salaryDto)
        {
            var employeeEmails = salaryDto.EmployeeEmails;
            var salaryBreakup = salaryDto.Salary;
            var basicSalaries = _salaryService.GetEmployeeBasic();

            foreach(var employee in employeeEmails)
            {
                salaryBreakup.EmployeeEmail = employee;
                salaryBreakup.Basic = basicSalaries.FirstOrDefault(emp => emp.EmailAddress == employee).Salary;
                salaryBreakup.TotalEarning = salaryBreakup.Basic + salaryBreakup.HRA + salaryBreakup.ShiftAllowance + salaryBreakup.TravelAllowance+ salaryBreakup.MiscellaneousCredit;
                salaryBreakup.TotalDeduction = salaryBreakup.PF + salaryBreakup.PT + salaryBreakup.MiscellaneousDebit;
                salaryBreakup.NetSalary = salaryBreakup.TotalEarning - salaryBreakup.TotalDeduction;
                salaryBreakup.ProcessedBy = User.FindFirst(ClaimTypes.Name)?.Value;

                _salaryService.CreditSalary(salaryBreakup);
            }

            return Json(new { success = true, message = "Payroll generated successfully!" });
        }

        public IActionResult GetPayroll(string duration)
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            var employee = _employeeService.GetEmployeeDetails(userEmail);
            var salary = _salaryService.GetEmployeeSalary(userEmail, duration);

            HttpContext.Session.SetString("PayrollPDFViewModel", JsonConvert.SerializeObject(new PayrollPDFViewModel
            {
                Salary = salary,
                Employee = employee
            }));

            // Return a JSON response indicating success
            return Json(new { success = true });
        }
        
        public IActionResult DisplayPayroll()
        {
            var payrollDataJson = HttpContext.Session.GetString("PayrollPDFViewModel");

            if (!string.IsNullOrEmpty(payrollDataJson))
            {
                var viewModel = JsonConvert.DeserializeObject<PayrollPDFViewModel>(payrollDataJson);
                return View("PayrollPDF", viewModel);
            }

            return Content("Failed. !!!");
        }

        public IActionResult PayrollPDF(string duration)
        {
            var pdfConverter = new SynchronizedConverter(new PdfTools());
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            var employee = _employeeService.GetEmployeeDetails(userEmail);
            var salary = _salaryService.GetEmployeeSalary(userEmail, duration);

            var viewModel = new PayrollPDFViewModel
            {
                Salary = salary,
                Employee = employee
            };

            var htmlContent = RenderViewToString("PayrollPDF", viewModel);

            //var pdfDocument = new HtmlToPdfDocument
            //{
            //    GlobalSettings = new GlobalSettings
            //    {
            //        ColorMode = ColorMode.Color,
            //        PaperSize = PaperKind.A4,
            //        Orientation = Orientation.Portrait
            //    },
            //    Objects = {
            //    new ObjectSettings
            //    {
            //        HtmlContent = htmlContent,
            //        WebSettings = { DefaultEncoding = "utf-8", LoadImages = true },
            //        LoadSettings = { BlockLocalFileAccess = false }
            //    }
            //}
            //};

            //byte[] pdf = pdfConverter.Convert(pdfDocument);
            //return File(pdf, "application/pdf", "Payslip.pdf");

            //var pdfConverter = new SynchronizedConverter(new PdfTools());

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                DocumentTitle = "Payslip",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdfDoc = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] pdf = pdfConverter.Convert(pdfDoc);
            return File(pdf, "application/pdf", $"Payslip-{DateTime.Now}.pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GeneratePDF()
        {
            var pdfConverter = new SynchronizedConverter(new PdfTools());

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                DocumentTitle = "Employee Details",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = "<html><body><h1>Hello, Employee!</h1></body></html>",
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdfDoc = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var pdf = pdfConverter.Convert(pdfDoc);
            return File(pdf, "application/pdf", "EmployeeDetails.pdf");
        }

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                if (viewResult.View == null) throw new ArgumentNullException($"{viewName} not found");

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext).Wait();
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
