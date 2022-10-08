using DBL.Models;
using DBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TimeTrackerAdmin.Controllers
{
    public class MaintenanceController : BaseController
    {
        private Bl bl;
        private string logFile;

        public MaintenanceController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        [HttpGet]
        public async Task<IActionResult> Subscriptions()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProjectCategories()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Currencies()
        {
            return View();
        }
    }
}
