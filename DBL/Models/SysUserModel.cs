using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class SysUserModel:GenericModel
    {
        public int UserCode { get; set; }
        public int ProfileCode { get; set; }
        public string UserName { get; set; }
        public string FullNames { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLogin { get; set; }
        public int UserStat { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string ProfileName { get; set; }
        public string Maker { get; set; }
    }
}
