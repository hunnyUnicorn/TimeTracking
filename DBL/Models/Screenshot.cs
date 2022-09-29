using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Screenshot
    {
        public int ScrCode { get; set; }
        public string ScrName { get; set; }
        public int DevCode { get; set; }
        public int ProjCode { get; set; }
        public DateTime ScrDate { get; set; }
        public string base64String { get; set; }
        public int TTCode { get; set; }
    }
    public class screenshotdets
    {
        public int ScrCode { get; set; }
        public string ScrName { get; set; }
        public string DeveloperName { get; set; }
        public string ProjName { get; set; }
        public DateTime TimeTaken { get; set; }
    }
}
