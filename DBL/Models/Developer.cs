using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Developer:BaseEntity
    {
        [JsonProperty("dcode")]
        public int DevCode { get; set; }
        [JsonProperty("fname")]
        public string FirstName { get; set; }
        [JsonProperty("mname")]
        public string MiddleName { get; set; }
        [JsonProperty("sname")]
        public string Surname { get; set; }
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
    }
    public class DevLogin
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
