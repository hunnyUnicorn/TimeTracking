using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBL.Models;
using DBL.Enums;

namespace DBL.Models
{
    public class UserModel
    {
        public int UserCode { get; set; }
        public int ProfileCode { get; set; }
        public string FullNames { get; set; }
        public bool ChangePass { get; set; }
        public int RespStatus { get; set; }
        public string RespMessage { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string LoginTime { get; set; }
        public List<Viewmenus> ProfileAccessData { get; set; }
    }
    public class Viewmenus : BaseEntity
    {
        public int MenuId { get; set; }
        public int MenuCode { get; set; }
        public string MenuName { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public string IconName { get; set; }
        public string OrderBy { get; set; }
        public int GroupCode { get; set; }
        public int MenuLevel { get; set; }
        public int ParentId { get; set; }
        public bool StatusId { get; set; }
    }
    public class UserAccountModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserCode { get; set; }

        [Required()]
        [Display(Name = "Profile Name")]
        public int ProfileCode { get; set; }
        public new string FullNames { get; set; }
        public new UserLoginStatus UserStatus { get; set; }
        public int UserStat { get; set; }
        public new int UserType { get; set; }
        public int UserRole { get; set; }
        public int RespStatus { get; set; }
        public string RespMessage { get; set; }
        public string UserStatusName { get; set; }
        public string UserProfileName { get; set; }
        public string UserRoleName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public DateTime CreateDate { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public string Extra6 { get; set; }
        

    }

    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Username", Prompt = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        public string Password { get; set; }
    }

    public class ViewAdminRights : BaseEntity
    {
        public int Id { get; set; }
        public int MenuCode { get; set; }
        public string MenuName { get; set; }
        public int ProfileCode { get; set; }
        public int StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public int AuthStat { get; set; }
        public string StatName { get; set; }
        public string AuthStatus { get; set; }
        public string RoleName { get; set; }
        public string Maker { get; set; }
        public int stage { get; set; }
        public string Accept { get; set; }
        public string Reject { get; set; }
    }

    public class UserGroupRightView : GenericModel
    {
        public int Id { get; set; }
        public int RightId { get; set; }
        public int MenuCode { get; set; }
        public string MenuName { get; set; }
        public int ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public bool StatusId { get; set; }
        public int MenuLevel { get; set; }
    }

    public class ViewUserList : BaseEntity
    {
        public int Id { get; set; }
        public int UserCode { get; set; }
        public int GroupCode { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string FullNames { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public int UserStat { get; set; }
        public string StatName { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public int AuthStat { get; set; }
        public string StatusName { get; set; }
        public string AuthStatus { get; set; }
        public int Attempts { get; set; }
        public DateTime PwdChangeDate { get; set; }
        public string Maker { get; set; }
        public int stage { get; set; }
        public string Accept { get; set; }
        public string Reject { get; set; }
    }

    public class ViewUserGroups : BaseEntity
    {
        public int Id { get; set; }
        public int ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public string GroupMail { get; set; }
        public string Descr { get; set; }
        public int ProfileStat { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public int AuthStat { get; set; }
        public string StatName { get; set; }
        public string ProfileStatus { get; set; }
        public string RoleName { get; set; }
        public int Users { get; set; }
        public string Maker { get; set; }
        public int stage { get; set; }
        public string Accept { get; set; }
        public string Reject { get; set; }
    }

    public class UserList
    {
        public int UserCode { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string FullNames { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Email { get; set; }
        public string StatusName { get; set; }
        public int UserStat { get; set; }
    }


    public class ChangeUserPassModel
    {
        [Required]
        public int UserCode { get; set; }

        [Required]
        [Display(Name = "Old Password", Prompt = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password", Prompt = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Re-type Password", Prompt = "Re-type Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do no match!")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetUserPassModel
    {
        [Required]
        public int UserCode { get; set; }

        [Required]
        [Display(Name = "OldPassword", Prompt = "Reset Code")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password", Prompt = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Re-type Password", Prompt = "Re-type Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do no match!")]
        public string ConfirmPassword { get; set; }
    }

    public class PasswordPolicyModel
    {
        public string PolicyText { get; set; }
    }

    public class ResetPassModel
    {
        [Required]
        [Display(Name = "Username", Prompt = "Username")]
        public string Username { get; set; }
    }
}
