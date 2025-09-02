using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeePortal.Core.Interfaces;
using EmployeePortal.Data.Data;
using EmployeePortal.Data.Repositories;
using EmployeePortal.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/SignIn";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Your services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<ISalaryRepository, SalaryRepository>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IEmailService, EMailService>();
builder.Services.AddScoped<IPortalHelper, PortalHelper>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapGet("/GoogleSignInUp", async context =>
{
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
        new AuthenticationProperties { RedirectUri = "/HandleGoogleSignInUpAsync" });
});

app.MapControllerRoute(
    name: "rootSignIn",
    pattern: "/",
    defaults: new { controller = "Employee", action = "SignIn" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=SignIn}/{id?}");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedRoles(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration.");
}

app.Run();