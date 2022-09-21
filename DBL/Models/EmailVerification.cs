using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class EmailVerification
    {
        public string userIdentifier { get; set; }
        public string VerifcationCode { get; set; }
    }
}
