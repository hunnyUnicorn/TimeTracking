using DBL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using TimeTrackerAdmin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc()
        .AddSessionStateTempDataProvider();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
{
    x.AccessDeniedPath = "/Account/AccessDenied/";
    x.LoginPath = "/Account/Login/";
});
var config = Util.GetAppConfig(builder.Configuration, builder.Environment);
builder.Services.Configure<AppConfig>(options =>
{
    options.ConnectionString = config.ConnectionString;
    options.DatabaseType = config.DatabaseType;
    options.WorkingDir = config.WorkingDir;
    options.LogFile = config.LogFile;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
