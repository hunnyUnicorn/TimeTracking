using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Subscription
    {
        public int PlanCode { get; set; }
        public string PlanName { get; set; }
        public decimal RenewalAmount { get; set; }
        public int PlanType { get; set; }
    }
}
