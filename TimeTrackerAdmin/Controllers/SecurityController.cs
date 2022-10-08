using DBL.Models;
using DBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TimeTrackerAdmin.Controllers
{
    public class SecurityController : Controller
    {
        private Bl bl;
        private string logFile;

        public SecurityController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        [HttpGet]
        public async Task<IActionResult> UserGroups()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Supervisor()
        {
            return View();
        }
    }
}
