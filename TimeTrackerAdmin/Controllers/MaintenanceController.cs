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
            var model = new List<Subscription>();
            try
            {
                model = (await bl.GetSubscriptionsAsync()).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Maintenance.Subscriptions()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ProjectCategories()
        {
            var model = new List<ProjectCategory>();
            try
            {
                model = (await bl.GetProjectCategoriesAsync()).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Maintenance.ProjectCategories()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Currencies()
        {
            return View();
        }
    }
}
