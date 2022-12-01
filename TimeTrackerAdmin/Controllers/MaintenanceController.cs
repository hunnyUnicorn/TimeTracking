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
            var model = new List<Currency>();
            try
            {
                model = (await bl.GetCurrenciesAsync()).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Maintenance.Currencies()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Currency()
        {
            var model = new Currency();
            return PartialView("_currency", model);
        }
        [HttpPost]
        public async Task<IActionResult> Currency(Currency model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var result = await bl.CreateCurrency(model);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                    reqResult.Success = true;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                        reqResult.Message = result.RespMessage;
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Developer.CreateInvoice", new Exception(result.RespMessage));
                        reqResult.Message = "Action failed due to a database error!";
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.CreateInvoice", ex);
                reqResult.Message = "Action failed due to a database error!";
            }
            return Json(reqResult);
        }
    }
}
