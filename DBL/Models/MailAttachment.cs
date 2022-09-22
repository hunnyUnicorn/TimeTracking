using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class MailAttachment
    {
        public Stream ItemStream { get; set; }
        public string ItemName { get; set; }
        public string MediaType { get; set; }
    }
}
