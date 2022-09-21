using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class UserDataModel
    {
        public string FullNames { get; set; }
        public string UserName { get; set; }
        public int userAgent { get; set; }
        public string SessionNo { get; set; }
        public int UserCode { get; set; }
        public int UserStatus { get; set; }
        public string Title { get; set; }
        public string LoginTime { get; set; }
        public int ClientCode { get; set; }
    }
}
