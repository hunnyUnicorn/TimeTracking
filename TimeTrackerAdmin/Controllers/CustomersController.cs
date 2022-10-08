using DBL.Models;
using DBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TimeTrackerAdmin.Controllers
{
    public class CustomersController : BaseController
    {
        private Bl bl;
        private string logFile;

        public CustomersController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        [HttpGet]
        public async Task<IActionResult> Clients()
        {
            var model = new List<Client>();
            try
            {
                model = (await bl.GetClients()).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Customers.Clients()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Developers()
        {
            var model = new List<Developer>();
            try
            {
                model = (await bl.GetDevelopersAsync()).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Customers.Clients()", ex);
            }
            return View(model);
        }
    }
}
