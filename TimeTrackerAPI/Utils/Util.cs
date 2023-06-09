﻿using Microsoft.AspNetCore.Hosting;
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

namespace TimeTrackerAPI
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

        public static UserModel GetCurrentUserData(IEnumerable<ClaimsIdentity> claims)
        {
            string userData = claims.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "userData")).FindFirst("userData").Value;
            if (string.IsNullOrEmpty(userData))
                return null;
            var userObject = JsonConvert.DeserializeObject<UserModel>(userData);
            return userObject ?? new UserModel(); ;
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
