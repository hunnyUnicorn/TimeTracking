using DBL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Entities
{
    public class Profile: BaseEntity
    {
        [NotMapped]
        public string TableName { get { return "Profiles"; } }

        [Column("ProfileId")]
        public int Id { get; set; }

        [Column("ProfileCode")]
        [Display(Name = "Profile Code")]
        [Key]
        public int ProfileCode { get; set; }

        [Column("ProfileName")]
        [Required()]
        [StringLength(20)]
        [Display(Name = "Profile Name")]
        public string ProfileName { get; set; }

        [Column("Description")]
        [Required()]
        [StringLength(50)]
        [Display(Name = "Profile Descr.")]
        public string Descr { get; set; }

        [Column("GroupMail")]
        [StringLength(50)]
        [Display(Name = "Group E-Mail")]
        [Required()]
        [DataType(DataType.EmailAddress)]
        public string GroupMail { get; set; }

        [Column("CreateDate")]
        [Display(Name = "Created Date")]
        public DateTime CreateDate { get; set; }

        [Column("ProfileStat")]
        [Display(Name = "ProfileStat")]
        public int ProfileStat { get; set; }       
        public int CreatedBy { get; set; }       
        public string  Maker { get; set; }       
        public string StatusName { get; set; } 
        public string Users { get; set; }  


    }
}
