using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class AuthToken
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
    public class AuthTokenResp
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}
