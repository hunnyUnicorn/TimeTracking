using DBL.Models;
using DBL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface ISecurityRepository
    {

        #region Users   
        Task<IEnumerable<GenericModel>> UserLoginAsync(int userCode, int status);
        Task<GenericModel> VerifyAsync(string userName);
        Task<SysUserModel> GetUserAsync(int code);
        Task<User> GetUser(int code);
        Task<IEnumerable<SysUserModel>> GetUsersAsync(int groupCode, string query = "");
        Task<IEnumerable<SysUserModel>> GetUsersAllAsync();
        Task<GenericModel> CreateUserAsync(User user, string password);
        Task<GenericModel> ModifyUserAsync(User user);
        Task<GenericModel> GetUserPasswordAsync(int userCode);
        Task<BaseEntity> ChangeUserPasswordAsync(int userCode, string password, string salt,string newPwd);
        Task<BaseEntity> ResetUserPasswordAsync(int userCode, string salt, string password, string newPass, int resetBy);
        Task<BaseEntity> ChangeUserStatusAsync(int userCode, int status, int changedBy);
        #endregion
        #region Profile
        Task<GenericModel> CreateProfileAsync(Profile group);
        Task<GenericModel> ModifyProfileAsync(Profile group);
        Task<GenericModel> ProfileCreateAccess(int pacode, int CreatedBy, int stat);
        Task<Profile> GetProfileAsync(int code);
        Task<IEnumerable<Profile>> GetProfileListAsync(string query);
        Task<IEnumerable<Viewmenus>> GetProfileAccess(int code);
        Task<IEnumerable<ProfileAccess>> GetProfileAccessListAsync(int code);
        Task<ProfileAccess> GetProfileAccessViewAsync(int code);
        Task<GenericModel> ChangeProfileStatusAsync(int profileCode, int status, int changedBy);
        #endregion
        #region menus
        Task<GenericModel> ProfileAcessmenusSynchronizeAsync(int ProfileCode, int Makerid);
        #endregion
        #region Audit Trail
        Task<IEnumerable<ViewAuditTrail>> GetAuditTrailsAsync(AuditFilterModel req);
        Task<BaseEntity> CreateAuditActionAsync(UserAudit data);
        #endregion
    }
}
