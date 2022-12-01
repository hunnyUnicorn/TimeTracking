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
    public class RenewSub
    {
        public int SubPlanCode { get; set; }
    }
    public class StripeDets:BaseEntity
    {
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string currency { get; set; }
        public string ClientReferenceId { get; set; }
        public string mode { get; set; }
    }
    public class SubPlanDets
    {
        public int PlanCode { get; set; }
        public string PlanName { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
