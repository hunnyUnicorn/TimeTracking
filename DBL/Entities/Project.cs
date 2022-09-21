using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBL.Entities
{
    [Table("Projects")]

    public class Project
    {
        [NotMapped]
        public static string TableName { get { return "Users"; } }

        [Column("Id")]
        public int Id { get; set; }

        [Column("ProjectCode")]
        public int ProjectCode { get; set; }

        [Column("ProjectName")]
        [Required()]
        [StringLength(50)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Column("ProjectDescr")]
        [Required()]
        [StringLength(1000)]
        [Display(Name = "Project Description")]
        public string ProjectDescr { get; set; }

        [Column("ProjCatCode")]
        [Required()]
        [Display(Name = "Project Category")]
        public int ProjCatCode { get; set; }

        [Column("ClientCode")]
        public int ClientCode { get; set; }

        [Column("ProjectStat")]
        [Required()]
        public int ProjectStat { get; set; }

        [Column("ProjRef")]
        [Required()]
        public string ProjRef { get; set; }

        [Column("StartDate")]
        [Required()]
        [Display(Name = "Project Start")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        [Required()]
        [Display(Name = "Project End")]
        public DateTime EndDate { get; set; }

        [Column("NotifType")]
        [Required()]
        [Display(Name = "Notification Type")]
        public int NotifType { get; set; }


        [Column("Budget")]
        [Required()]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Column("Priority")]
        [Required()]
        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
    }

    public class ProjectInviteModel
    {
        public string Email { get; set; }
        public int ProjectCode { get; set; }
        public int ClientCode { get; set; }
    }
}
