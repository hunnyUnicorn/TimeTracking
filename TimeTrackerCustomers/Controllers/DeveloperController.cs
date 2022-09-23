﻿using DBL;
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
       
    }
}
