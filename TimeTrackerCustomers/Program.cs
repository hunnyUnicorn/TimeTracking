using DBL.Models;
using FastReport.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Rotativa.AspNetCore;
using Stripe;
using TimeTrackerCustomers;
using TimeTrackerCustomers.Utils;

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
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider2(builder.Configuration["ImagesFolder"]),
    RequestPath = new PathString("/ScreenCasts"),
});
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider2(builder.Configuration["DesktopAppFolder"]),
    RequestPath = new PathString("/DesktopApp"),
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
IWebHostEnvironment env = app.Environment;
RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);
StripeConfiguration.ApiKey = "sk_test_51Lnd7iSI2vuBxoF6mqrfp7jMDp4FjvChhD0R4zTSqbjSdqjSm7r1tQovqKAxTI2hlOHA6ogPviPze8CwjsdmhB4i00X45J25EC";
FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
app.Run();
