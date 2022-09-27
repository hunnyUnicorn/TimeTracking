using DBL;
using DBL.Enums;
using DBL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TimeTrackerCustomers.Components
{
    public class NotificationsViewComponent:ViewComponent
    {
        private Bl bl;
        private string logFile;

        public NotificationsViewComponent(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        public async Task<IViewComponentResult> InvokeAsync(int CustCode,USER_TYPE userType)
        {
            var model = new List<Notification>();
            try
            {
                model = (await bl.GetNotificationsAsync(CustCode,userType)).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "NotificationsViewComponent.InvokeAsync", ex);
            }
            return View(model);
        }
    }
}
