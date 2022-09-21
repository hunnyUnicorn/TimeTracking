using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class SupervisorQueueModel
    {
        public int ActionCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string ActionType { get; set; }
        public string GroupName { get; set; }
        public string Maker { get; set; }
        public int RoleCode { get; set; }
        public int MCCaategory { get; set; }
        public string Details { get; set; }
    }
}
