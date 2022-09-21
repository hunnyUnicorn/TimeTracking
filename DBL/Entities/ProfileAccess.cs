using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBL.Models;

namespace DBL.Entities
{
    public class ProfileAccess : BaseEntity
    {
        [NotMapped]
        public string TableName { get { return "ProfileAccess"; } }
        [Column("ProfileAccessId")]
        public int Id { get; set; }

        [Column("ProfileAccessCode")]
        [Display(Name = "Profile Access Code")]
        [Key]
        public int ProfileAccessCode { get; set; }

        [Column("MenuCode")]
        [Display(Name = "Menu Code")]
        public int MenuCode { get; set; }

        [Column("ProfileCode")]
        [Display(Name = "Profile Code")]
        public int ProfileCode { get; set; }

        [Column("Granted")]
        [Display(Name = "Grant Access")]
        public bool Granted { get; set; }

        [Column("CreateDate")]
        [Display(Name = "Created Date")]
        public DateTime CreateDate { get; set; }

        [Column("ProfileAccessStat")]
        [Display(Name = "ProfileAccessStat")]
        public int ProfileAccessStat { get; set; }

        [Display(Name = "Profile Name")]
        public string ProfileName { get; set; }

        [Display(Name = "Main Menu")]
        public string ParentName { get; set; }

        [Display(Name = "Sub Menu")]
        public string MenuName { get; set; }
       
        public string StatusName { get; set; }
        public int MenuLevel { get; set; }
        public int stat { get; set; }
        public string statBtn { get; set; }
        public string Data1 { get; set; }
        public int CreatedBy { get; set; }


    }
}
