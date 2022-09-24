using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class TimeTrack
    {
        public int TTCode { get; set; }
        public string TTDescr { get; set; }
        public int DevCode { get; set; }
        public int ProjectCode { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProjectName { get; set; }
        public int KeyboardHits { get; set; }
        public int MouseClicks { get; set; }
    }
}
