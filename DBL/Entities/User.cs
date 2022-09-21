using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBL.Models;

namespace DBL.Entities
{
    [Table("Users")]
    public class User:BaseEntity
    {
        [NotMapped]
        public static string TableName { get { return "Users"; } }

        [Column("Id")]
        public int Id { get; set; }

        [Column("UserCode")]
        public int UserCode { get; set; }

        [Column("UserName")]
        [Required()]
        [StringLength(20)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Column("FullNames")]
        [Required()]
        [StringLength(35)]
        [Display(Name = "Full Names")]
        public string FullNames { get; set; }

        [Column("PhoneNo")]
        [Required()]
        [StringLength(15)]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Column("Email")]
        [Required()]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail Address")]
        public string Email { get; set; }

        [Column("Pwd")]
        [StringLength(100)]
        public string Pwd { get; set; }

        [Column("Salt")]
        [StringLength(100)]
        public string Salt { get; set; }

        [Column("UserStat")]
        public int UserStat { get; set; }

        [Column("UserRole")]
        [Required()]
        [Display(Name = "User Role")]
        public int UserRole { get; set; }

        [Column("LastLogin")]
        public DateTime LastLogin { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }

        [Column("ChangePwd")]
        public bool ChangePwd { get; set; }

        [Column("Attempts")]
         public int Attempts { get; set; }

        [Column("Token")]
        [StringLength(50)]
        public string Token { get; set; }

        [Column("ProfileCode")]
        [Required()]
        public int ProfileCode { get; set; }
        public string  ProfileName { get; set; }

        public int CreatedBy { get; set; }
    }
}
