using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class ReqResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public dynamic Data { get; set; }
    }
}
