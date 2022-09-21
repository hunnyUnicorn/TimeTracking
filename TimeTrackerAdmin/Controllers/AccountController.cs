using DBL;
using DBL.Enums;
using DBL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace TimeTrackerAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : BaseController
    {
        private Bl bl;
        private string logFile;

        public AccountController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    var userModel = await bl.UserLoginAsync(model.Username, model.Password);
                    if (userModel.RespStatus == 0)
                    {
                        //---- Check if change password
                        if (userModel.ChangePass)
                        {
                            return RedirectToAction("ChangePass", new { userCode = userModel.UserCode, returnUrl = returnUrl });
                        }
                        else
                        {
                            SetUserLoggedIn(userModel, false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    else
                    {
                        if (userModel.RespStatus == 1)
                        {
                            Danger(userModel.RespMessage);
                        }
                        else
                        {
                            LogUtil.Error(logFile, "Account.Login", new Exception(userModel.RespMessage));
                            Danger("Login failed due to a database error!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(logFile, "Account.Login", ex);
                    Danger("Login failed due to an error!");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePass(int userCode)
        {
            ChangeUserPassModel passModel = new ChangeUserPassModel { UserCode = userCode };
            return View(passModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePass(ChangeUserPassModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = await bl.ChangeUserPasswordAsync(model);
                if (userModel.RespStatus == 0)
                {
                    Success("Password changed successfully.");
                }
                else
                    Danger(userModel.RespMessage);
            }
            else
                Danger("Data model is NOT valid!");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userCode = SessionUserData.UserCode;
            ChangeUserPassModel passModel = new ChangeUserPassModel { UserCode = userCode };
            return View(passModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeUserPassModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var userModel = await bl.ChangeUserPasswordAsync(model);
                if (userModel.RespStatus == 0)
                {
                    Success("Password changed successfully.");
                }
                else
                    Danger(userModel.RespMessage);
            }
            else
            {
                Danger("Data model is NOT valid!");
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPass(string returnUrl = null)
        {
            Warning("Under Development");
            return RedirectToAction("Login");
            //ViewData["ReturnUrl"] = returnUrl;
            //return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPass(ResetPassModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        private async void SetUserLoggedIn(UserModel user, bool rememberMe)
        {

            //string userData = JsonConvert.SerializeObject(serializeModel);
            user.LoginTime = DateTime.Now.ToString("HH:mm:ss");
            string userData = JsonConvert.SerializeObject(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserCode.ToString(),"NameIdentifier"),
                new Claim(ClaimTypes.Name, user.FullNames,"Name"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("userData", userData),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");

            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity[] { claimsIdentity });
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = new DateTimeOffset?(DateTime.UtcNow.AddMinutes(30))
            });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/")
                return RedirectToAction(nameof(HomeController.Dashboard), "Home", new { area = "" });

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Dashboard), "Home", new { area = "" });
            }
        }

        #endregion
        #region SignUp
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(Developer model)
        {
            try
            {
                var resp = await bl.DeveloperSignUp(model);
                if (resp.RespStatus == 0)
                {
                    return RedirectToAction("Login");
                }
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Developer.SignUp", ex);
            }
            return View(model);
        }
        #endregion
    }
}
