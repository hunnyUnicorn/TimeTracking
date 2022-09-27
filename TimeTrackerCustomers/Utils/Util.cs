using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DBL.Models;
using Taiwac.Tools;

namespace TimeTrackerCustomers
{
    public class Util
    {
        public static AppConfig GetAppConfig(IConfiguration config, IWebHostEnvironment env)
        {
            var appConfig = new AppConfig();
            var connConfig = config.GetSection("DbConnData");
            var connData = connConfig.Get<DbConnData>();

            appConfig.DatabaseType = connData.DbType;
            if (connData.DbType == 0)
            {
                var conMan = new ConnectionManager();
                string connString = conMan.GetConnectionString(connData.Data, connData.Id, connData.Key);
                appConfig.ConnectionString = connString;
            }

            appConfig.LogFile = GetLogFile(env);
            return appConfig;
        }
      
        public static string GetLogFile(IWebHostEnvironment env)
        {
            try
            {
                string logDir = Path.Combine(env.ContentRootPath, "logs");

                //---- Create Directory if it does not exist              
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                return Path.Combine(logDir, "ErrorLog.log");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static Developer GetCurrentDeveloperData(IEnumerable<ClaimsIdentity> claims)
        {
            string userData = claims.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "developerData")).FindFirst("developerData").Value;
            if (string.IsNullOrEmpty(userData))
                return null;
            var userObject = JsonConvert.DeserializeObject<Developer>(userData);
            return userObject ?? new Developer(); ;
        }
        public static Client GetCurrentClientData(IEnumerable<ClaimsIdentity> claims)
        {
            string userData = claims.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "clientData")).FindFirst("clientData").Value;
            if (string.IsNullOrEmpty(userData))
                return null;
            var userObject = JsonConvert.DeserializeObject<Client>(userData);
            return userObject ?? new Client(); ;
        }
        public static string FormatDateDifference(int milliseconds)
        {
            if(milliseconds> 8.64e+7)
            {
                return String.Format("{0}days ago", (milliseconds / 8.64e+7));
            }
            if (milliseconds > 3.6e+6)
            {
                return String.Format("{0} hours ago", (milliseconds / 3.6e+6));
            }
            if (milliseconds > 60000)
            {
                return String.Format("{0} minutes ago", (milliseconds / 60000));
            }
            if (milliseconds > 10000)
            {
                return String.Format("{0} seconds ago", (milliseconds / 10000));
            }
            return "";
        }
    }

    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";
        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
        public string IconClass { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }
}
