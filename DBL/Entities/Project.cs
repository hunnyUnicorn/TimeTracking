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
        [Column("CCYCode")]
        [Required()]
        [Display(Name = "Currency")]
        public int CCYCode { get; set; }

        [Column("ClientCode")]
        public int ClientCode { get; set; }

        [Column("ProjectStat")]
        [Required()]
        public int ProjectStat { get; set; }

        [Column("ProjRef")]
        [Required()]
        public string ProjRef { get; set; }

        [Column("ProjStartDate")]
        [Required()]
        [Display(Name = "Project Start")]
        public DateTime ProjStartDate { get; set; }

        [Column("ProjEndDate")]
        [Required()]
        [Display(Name = "Project End")]
        public DateTime ProjEndDate { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public int Members { get; set; }
        [NotMapped]
        public string CCYName { get; set; }
    }

    public class ProjectInviteModel
    {
        public string Email { get; set; }
        public int ProjectCode { get; set; }
        public int ClientCode { get; set; }
    }
}
