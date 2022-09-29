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

namespace TimeTrackerCustomers.Controllers
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

                    var userType = await bl.Login(model);
                    switch(userType)
                    {
                        case 1: // Developer
                            var developer = await bl.VerifyDeveloper(model);
                            if(developer.RespStatus==0)
                            {
                                SetDeveloperLogin(developer,false);
                                return RedirectToAction("Dashboard", "Developer");
                            }
                            else
                            {
                                Danger(developer.RespMessage);
                            }
                            break;
                        case 2: // Client
                            var client = await bl.VerifyClient(model);
                            if (client.RespStatus == 0)
                            {
                                SetClientLogin(client, false);
                                return RedirectToAction("Dashboard", "Client");
                            }
                            else
                            {
                                Danger(client.RespMessage);
                            }
                            break;
                        case 3:// Both
                            return RedirectToAction("ChooseAccount", "Home", new { Email = model.Username });
                            break;
                        default:
                            Danger("Invalid email/password");
                            break;
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
            var userCode = 0;
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        private async void SetDeveloperLogin(Developer user, bool rememberMe)
        {

            //string userData = JsonConvert.SerializeObject(serializeModel);
            //user.LoginTime = DateTime.Now.ToString("HH:mm:ss");
            string userData = JsonConvert.SerializeObject(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.DevCode.ToString(),"NameIdentifier"),
                new Claim(ClaimTypes.Name, user.FirstName,"Name"),
                new Claim(ClaimTypes.Role, "Developer"),
                new Claim("developerData", userData),
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
        private async void SetClientLogin(Client user, bool rememberMe)
        {

            //string userData = JsonConvert.SerializeObject(serializeModel);
            //user.LoginTime = DateTime.Now.ToString("HH:mm:ss");
            string userData = JsonConvert.SerializeObject(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ClientCode.ToString(),"NameIdentifier"),
                new Claim(ClaimTypes.Name, user.ClientName,"Name"),
                new Claim(ClaimTypes.Role, "Client"),
                new Claim("clientData", userData),
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
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(PasswodResetModel model)
        {
            try
            {
                var resp = await bl.ResetPasswordLink(model);
                if(resp.RespStatus==0)
                {
                    Success(resp.RespMessage);
                    return RedirectToAction("Login", "Account");
                }
                else if(resp.RespStatus==1)
                {
                    Danger(resp.RespMessage);
                }
                else
                {
                    LogUtil.Error(logFile, "Account.ForgotPassword", new Exception(resp.RespMessage));
                    Danger("Database Error occured when executing your request");
                }
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Account.ForgotPassword", ex);
                Danger("Technical error occured when executing your request");
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var validateResp = await bl.ValidateLink(id);
            var model = new PasswodResetModel();
            model.ResetLinkIdentifier = id;
            if (validateResp.RespStatus==0)
            {
                return View(model);
            }
            else if (validateResp.RespStatus == 1)
            {
                Danger(validateResp.RespMessage);
            }
            else
            {
                LogUtil.Error(logFile, "Account.ForgotPassword", new Exception(validateResp.RespMessage));
                Danger("Database Error occured when executing your request");
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(PasswodResetModel model)
        {
            try
            {
                if(!model.Password.Equals(model.ConfirmPassword))
                {
                    Danger("Password should be the same as confirmation password");
                    return View(model);
                }
                var resp = await bl.ResetPassword(model);
                if (resp.RespStatus == 0)
                {
                    Success(resp.RespMessage);
                    return RedirectToAction("Login", "Account");
                }
                else if (resp.RespStatus == 1)
                {
                    Danger(resp.RespMessage);
                }
                else
                {
                    LogUtil.Error(logFile, "Account.ResetPassword", new Exception(resp.RespMessage));
                    Danger("Database Error occured when executing your request");
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Account.ResetPassword", ex);
                Danger("Technical error occured when executing your request");
            }
            return View(model);
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
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
            }
        }

        #endregion
    }
}
