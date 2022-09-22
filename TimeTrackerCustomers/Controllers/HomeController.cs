using DBL;
using DBL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using TimeTrackerCustomers.Models;

namespace TimeTrackerCustomers.Controllers
{
    public class HomeController : Controller
    {
        Bl bl;
        private string logFile;
        public HomeController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> SignUpDeveloper(Developer model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var result = await bl.CreateDeveloper(model);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                   
                    reqResult.Success = true;

                    reqResult.Data = result.Data1;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                       // Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Home.SignUpDeveloper", new Exception(result.RespMessage));
                       // Danger("Action failed due to a database error!");
                    }
                }
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Home.SignUpDeveloper()", ex);
            }
            return Json(reqResult);
        }
        [HttpPost]
        public async Task<JsonResult> SignUpClient(Client model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var result = await bl.CreateClient(model);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                    reqResult.Data = result.Data1;
                    reqResult.Success = true;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                        // Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Home.SignUpClient", new Exception(result.RespMessage));
                        // Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Home.SignUpClient()", ex);
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> EmailVerification(string useridentifier)
        {
            EmailVerification model = new EmailVerification();
            model.userIdentifier=useridentifier;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EmailVerification(EmailVerification model)
        {
            try
            {
                var resp = await bl.VerifyEmail(model);
                if(resp.RespStatus==0)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Home.SignUpClient()", ex);
            }
            return View(model);
        }
    }
}