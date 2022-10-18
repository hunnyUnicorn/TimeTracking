using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class Invoice
    {
        public int InvoiceCode { get; set; }
        public string InvoiceRef { get; set; }
        public int InvoiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalHours { get; set; }
        public decimal HourRate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ProjectCode { get; set; }
        public int DeveloperCode { get; set; }
        public DateTime DateGenerated { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
    }
}
