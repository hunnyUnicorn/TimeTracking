using DBL.Models;
using DBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TimeTrackerAdmin.Controllers
{
    public class DeveloperController : Controller
    {
        private Bl bl;
        private string logFile;

        public DeveloperController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        public IActionResult Projects()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult MyActivities()
        {
            return View();
        }
        public IActionResult WebTracker()
        {
            return View();
        }
    }
}
