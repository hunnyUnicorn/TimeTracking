using DBL.Models;
using DBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DBL.Entities;
using DBL.Enums;

namespace TimeTrackerAdmin.Controllers
{
    public class SecurityController : BaseController
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
        public async Task<IActionResult> UserGroups(string query="")
        {
            var model = new List<Profile>();
            try
            {
                model = (await bl.GetProfilesAsync(query)).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Security.UserGroups()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Supervisor(int groupcode = 0, int typ = 0)
        {
            var data = await bl.GetMySupervisorQueueAsync(SessionUserData.UserCode, typ, McCategory.Supervisor, groupcode);
            return View(data);
        }
    }
}
