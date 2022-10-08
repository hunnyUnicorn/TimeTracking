using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Client : BaseEntity
    {
        [JsonProperty("ccode")]
        public int ClientCode { get; set; }
        [JsonProperty("cname")]
        public string ClientName { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonIgnore]
        public string Pwd { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        [JsonProperty("pass")]
        public string RawPass { get; set; }
        [JsonIgnore]
        public string UserIdentifier { get; set; }
        public decimal RenewalAmount { get; set; }
        public DateTime NextRenewalDate { get; set; }
        public string PlanName { get; set; }
    }
    public class ClientLogin
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
