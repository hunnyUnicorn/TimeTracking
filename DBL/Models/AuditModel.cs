using DBL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Models
{
    public class AuditFilterModel : AuditClientFilterModel
    {
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        [Display(Name = "UserName")]
        public int UserCode { get; set; }

        public int MakerId { get; set; }
        public int ModuleId { get; set; }
    }
    public class AuditClientFilterModel
    {
        [Display(Name = "Client")]
        public int ClientCode { get; set; }

        [Display(Name = "Operator")]
        public int OperatorCode { get; set; }
    }
    public class UserAudit : BaseEntity
    {
        public int Id { get; set; }
        public int UserCode { get; set; }
        public string ActionDescr { get; set; }
        public DateTime AuditDate { get; set; }

        public string ClntIP { get; set; }
        public string Browser { get; set; }
        public int ModuleId { get; set; }

        public string Location_ { get; set; }
        public string MdlFunction { get; set; }

        //string Browser, string ActionDescr, int moduleid, string MdlFunction, int UserCode
    }
    public class ViewAuditTrail : BaseEntity
    {
        [Display(Name = "AdtTrlId")]
        public int AdtTrlId { get; set; }
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "User Code")]
        public int UserCode { get; set; }
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Display(Name = "Audit Action")]
        public string ActionDescr { get; set; }
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        [Display(Name = "Audit Date")]
        public DateTime AuditDate { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        [Display(Name = "Browser")]
        public string Browser { get; set; }
        public string Location_ { get; set; }
        public string rdc { get; set; }
        public string SData { get; set; }
        public int MakerId { get; set; }

        public string OperatorName { get; set; }
        public string ClientName { get; set; }

    }
}
