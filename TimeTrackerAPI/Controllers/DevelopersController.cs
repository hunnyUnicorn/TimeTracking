using BMobile.DBL.Models;
using DBL;
using DBL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TimeTrackerAPI.Controllers
{
    [ApiController]
    [Route("developers/api/v1/[action]")]
    public class DevelopersController : ControllerBase
    {
        private Bl bl;
        private string logFile;
        public DevelopersController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = appSett.Value.LogFile;
        }

        [HttpPost]
        public async Task<ApiResponseData> ProcessRequest(ApiRequestData model)
        {
            try
            {
                var result = await bl.ProcessDeveloperRequests(model);
                return result;
            }
            catch(Exception ex) 
            {
                LogUtil.Error(logFile, "Api.Agency.Operation", ex);
                return new ApiResponseData { RespStatus = 1, Message = "Request failed due to an error!" };
            }
        }
        
    }
}