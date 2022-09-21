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
    [Table("UserGroups")]
    public class UserGroup : BaseEntity
    {
        [NotMapped]
        public static string TableName { get { return "UserGroups"; } }

        [Column("Id")]
        public int Id { get; set; }

        [Column("GroupCode")]
        public int GroupCode { get; set; }

        [Column("GroupName")]
        [Required()]
        [StringLength(35)]
        [Display(Name = "Group Name")]
        //[RegularExpression("^[a-zA-Z0-9]*$")]
        public string GroupName { get; set; }

        [Column("GroupEmail")]
        [StringLength(50)]
        [Display(Name = "Group E-Mail")]
        //[DataType(DataType.EmailAddress)]
        public string GroupEmail { get; set; }

        [Column("GroupDescr")]
        [Required()]
        [StringLength(150)]
        [Display(Name = "Description")]
        //[RegularExpression("^[a-zA-Z0-9]*$")]
        public string GroupDescr { get; set; }

        [Column("GroupStat")]
        public int GroupStat { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }

        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("UserRole")]
        [Required()]
        [Display(Name = "User Role")]
        public int UserRole { get; set; }

        [Column("AuthStat")]
        public int AuthStat { get; set; }

        public string RoleName { get; set; }
        public string StatName { get; set; }
        public string AuthStatus { get; set; }
    }
}
