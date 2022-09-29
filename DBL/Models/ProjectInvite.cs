using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class ProjectInvite:BaseEntity
    {
        public int InviteCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescr { get; set; }
        public string ProjectCategory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; }
        public string CurencySymbol { get; set; }
        public decimal RatePerHour { get; set; }
    }
}
