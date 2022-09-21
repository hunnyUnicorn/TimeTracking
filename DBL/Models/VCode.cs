using DBL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class VCodeModel
    {
        public string useridentifier { get; set; }
        public string RawVCode { get; set; }
        public string VCode { get; set; }
        public string Salt { get; set; }
        public USER_TYPE UserType { get; set; }
    }
}
