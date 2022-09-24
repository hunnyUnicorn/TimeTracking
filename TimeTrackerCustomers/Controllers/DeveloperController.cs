using DBL;
using DBL.Entities;
using DBL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TimeTrackerCustomers.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DeveloperController : BaseController
    {
        private Bl bl;
        private string logFile;
        public DeveloperController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        [HttpGet]
        public async Task<IActionResult> DashBoard()
        {
            ViewBag.ScreenCasts = (await bl.GetDeveloperScreenShots(0,"0",SessionDeveloperData.DevCode)).ToList();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Projects()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProjectList(int status = 0)
        {
            var projectList = new List<Project>();
            projectList = (await bl.GetDeveloperProjects(SessionDeveloperData.DevCode)).ToList();
            return PartialView("_ProjectList", projectList);
        }
        [HttpGet]
        public async Task<IActionResult>Reports()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MyActivities()
        {
            var model = new List<TimeTrack>();
            try
            {
                model = (await bl.GetTimeTracksAsync(SessionDeveloperData.DevCode)).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Developer.MyActivities", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> WebTracker()
        {
            var model = await bl.GetDeveloperProjects(SessionDeveloperData.DevCode);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTimeFrame(TimeTrack model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                model.DevCode = SessionDeveloperData.DevCode;
                var result = await bl.CreateTimeFrame(model);
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
                        Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Developer.CreateTimeFrame", new Exception(result.RespMessage));
                        Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.CreateTimeFrame", ex);
            }
            return Json(reqResult);
        }
        [HttpPost]
        public async Task<IActionResult> StopTimeFrame(int TTCode, int KeyHits, int mouseHits)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var result = await bl.StopTimeFrame(TTCode,KeyHits,mouseHits);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                    reqResult.Success = true;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                        Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Developer.StopTimeFrame", new Exception(result.RespMessage));
                        Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.StopTimeFrame", ex);
            }
            return Json(reqResult);
        }
    }
}
