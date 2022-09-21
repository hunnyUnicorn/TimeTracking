using DBL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Security.Claims;

namespace TimeTrackerCustomers.Controllers
{
    public class BaseController : Controller
    {
        public Developer SessionDeveloperData
        {
            get
            {
                {
                    Developer userDataSerializeModel = null;
                    if (base.User is ClaimsPrincipal)
                    {
                        string claim = BaseController.GetClaim((base.User as ClaimsPrincipal).Claims.ToList<Claim>(), "developerData");
                        if (!string.IsNullOrEmpty(claim))
                        {
                            userDataSerializeModel = JsonConvert.DeserializeObject<Developer>(claim);
                        }
                    }

                    base.ViewData["DeveloperData"] = (userDataSerializeModel ?? new Developer());
                    if (userDataSerializeModel == null)
                    {
                        //string url = base.Url.Action("Login", "Account");
                        //requestContext.HttpContext.Response.Redirect(url);
                    }

                    return userDataSerializeModel;
                }
            }
        }
        public Client SessionClientData
        {
            get
            {
                {
                    Client userDataSerializeModel = null;
                    if (base.User is ClaimsPrincipal)
                    {
                        string claim = BaseController.GetClaim((base.User as ClaimsPrincipal).Claims.ToList<Claim>(), "clientData");
                        if (!string.IsNullOrEmpty(claim))
                        {
                            userDataSerializeModel = JsonConvert.DeserializeObject<Client>(claim);
                        }
                    }

                    base.ViewData["ClientData"] = (userDataSerializeModel ?? new Client());
                    if (userDataSerializeModel == null)
                    {
                        //string url = base.Url.Action("Login", "Account");
                        //requestContext.HttpContext.Response.Redirect(url);
                    }

                    return userDataSerializeModel;
                }
            }
        }

        public static string GetClaim(List<Claim> claims, string key)
        {
            Claim claim = claims.FirstOrDefault((Claim c) => c.Type == key);
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }

        public void Success(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Success, message, dismissable, "fa fa-check");
        }

        public void Information(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Information, message, dismissable, "fa fa-info-circle");
        }

        public void Warning(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Warning, message, dismissable, "fa fa-warning");
        }

        public void Danger(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Danger, message, dismissable, "fa fa-times-circle");
        }
        public string GetIP()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            string ip = remoteIpAddress.ToString();
            return ip;
        }
        private void AddAlert(string alertStyle, string message, bool dismissable, string iconClass)
        {
            var alerts = new List<Alert>();

            string jsonData = TempData.ContainsKey(Alert.TempDataKey) ? (string)TempData[Alert.TempDataKey] : "";
            if (!string.IsNullOrEmpty(jsonData))
                alerts = JsonConvert.DeserializeObject<List<Alert>>(jsonData);

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable,
                IconClass = iconClass
            });

            TempData[Alert.TempDataKey] = JsonConvert.SerializeObject(alerts);
        }
        public string[] GetModelErrors()
        {
            List<string> errors = new List<string>();
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                    errors.Add(error.ErrorMessage);
            }

            return errors.ToArray();
        }
    }
}
