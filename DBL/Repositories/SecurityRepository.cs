using Dapper;
using DBL.Models;
using DBL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class SecurityRepository:BaseRepository,ISecurityRepository
    {
        public SecurityRepository(string connectionString) : base(connectionString)
        {
        }
        #region Users
        public async Task<IEnumerable<GenericModel>> UserLoginAsync(int userCode, int status)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                parameters.Add("@LoginStat", status);
                return (await connection.QueryAsync<GenericModel>("sp_UserLogin", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<GenericModel> VerifyAsync(string userName)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                return (await connection.QueryAsync<GenericModel>("sp_UserValidity", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<SysUserModel> GetUserAsync(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", code);
                return (await connection.QueryAsync<SysUserModel>(FindStatement("vw_Users", "UserCode"), parameters)).FirstOrDefault();
            }
        }
        public async Task<User> GetUser(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from vw_Users where UserCode=" + code + "";
                connection.Open();
                return (await connection.QueryAsync<User>(sql)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<SysUserModel>> GetUsersAsync(int groupCode, string query = "")
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", groupCode);
                string sql = "Select * From vw_Users Where ProfileCode = @Id";
                if (!string.IsNullOrEmpty(query))
                    sql += " and UserName Like '%" + query + "%'";
                return (await connection.QueryAsync<SysUserModel>(sql, parameters)).ToList();
            }
        }

        public async Task<IEnumerable<SysUserModel>> GetUsersAllAsync()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                return (await connection.QueryAsync<SysUserModel>(GetAllStatement("vw_Users"))).ToList();
            }
        }
        public async Task<GenericModel> CreateUserAsync(User user, string password)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Username", user.UserName);
                parameters.Add("@ProfileCode", user.ProfileCode);
                parameters.Add("@FullNames", user.FullNames);
                parameters.Add("@Email", user.Email);
                parameters.Add("@PhoneNo", user.PhoneNo);
                parameters.Add("@Salt", user.Salt);
                parameters.Add("@Pwd", user.Pwd);
                parameters.Add("@CreatedBy", user.CreatedBy);
                parameters.Add("@RawPwd", password);
                return (await connection.QueryAsync<GenericModel>("sp_UserCreate", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> ModifyUserAsync(User user)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", user.UserCode);
                parameters.Add("@Username", user.UserName);
                parameters.Add("@ProfileCode", user.ProfileCode);
                parameters.Add("@FullNames", user.FullNames);
                parameters.Add("@Email", user.Email);
                parameters.Add("@PhoneNo", user.PhoneNo);
                parameters.Add("@Status", user.UserStat);
                parameters.Add("@CreatedBy", user.CreatedBy);
                return (await connection.QueryAsync<GenericModel>("sp_UserModify", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> GetUserPasswordAsync(int userCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                return (await connection.QueryAsync<GenericModel>("sp_UserGetPassword", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<BaseEntity> ChangeUserPasswordAsync(int userCode, string password, string salt,string newPwd)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                parameters.Add("@Pwd", password);
                parameters.Add("@NewPwd", newPwd);
                parameters.Add("@Salt", salt);
                return (await connection.QueryAsync<BaseEntity>("sp_UserChangePassword", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<BaseEntity> ResetUserPasswordAsync(int userCode, string salt, string password, string newPass, int resetBy)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                parameters.Add("@Salt", salt);
                parameters.Add("@Password", password);
                parameters.Add("@NewPass", newPass);
                parameters.Add("@ResetBy", resetBy);
                return (await connection.QueryAsync<BaseEntity>("sp_UserResetPassword", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<BaseEntity> ChangeUserStatusAsync(int userCode, int status, int changedBy)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                parameters.Add("@Stat", status);
                parameters.Add("@ActionBy", changedBy);
                return (await connection.QueryAsync<BaseEntity>("sp_UserChangeStatus", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        #endregion
        #region Profile
        public async Task<GenericModel> CreateProfileAsync(Profile group)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileName", group.ProfileName);
                parameters.Add("@Descr", group.Descr);
                parameters.Add("@Mail", group.GroupMail);
                parameters.Add("@CreatedBy", group.CreatedBy);
                parameters.Add("@Role", 1);
                return (await connection.QueryAsync<GenericModel>("sp_ProfileCreate", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> ModifyProfileAsync(Profile group)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileCode", group.ProfileCode);
                parameters.Add("@ProfileName", group.ProfileName);
                parameters.Add("@Descr", group.Descr);
                parameters.Add("@Mail", group.GroupMail);
                parameters.Add("@CreatedBy", group.CreatedBy);
                return (await connection.QueryAsync<GenericModel>("sp_ProfileModify", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> ProfileCreateAccess(int pacode, int CreatedBy, int stat)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileAccessCode", pacode);
                parameters.Add("@Stat", stat);
                parameters.Add("@MakerId", CreatedBy);
                return (await connection.QueryAsync<GenericModel>("SP_ProfileAccessAssign", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<Profile> GetProfileAsync(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "Select * from vw_Profiles where ProfileCode=" + code + "";
                connection.Open();
                return (await connection.QueryAsync<Profile>(sql)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<Profile>> GetProfileListAsync(string query)
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from vw_Profiles";
                if (!string.IsNullOrEmpty(query))
                    sql += " Where ProfileName like '%" + query + "%'";
                connection.Open();
                return (await connection.QueryAsync<Profile>(sql)).ToList();
            }
        }
        public async Task<IEnumerable<Viewmenus>> GetProfileAccess(int code)
        {
            using (var Connection = new SqlConnection(_connString))
            {
                Connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileCode", code);
                return (await Connection.QueryAsync<Viewmenus>("sp_ProfileAccess ", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<ProfileAccess>> GetProfileAccessListAsync(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileCode", code);
                return (await connection.QueryAsync<ProfileAccess>("SP_ProfileAccessRight ", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<ProfileAccess> GetProfileAccessViewAsync(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from vw_ProfileAccess where ProfileAccessCode=" + code + "";
                connection.Open();
                return (await connection.QueryAsync<ProfileAccess>(sql)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> ChangeProfileStatusAsync(int profileCode, int status, int changedBy)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileCode", profileCode);
                parameters.Add("@Stat", status);
                parameters.Add("@ActionBy", changedBy);
                return (await connection.QueryAsync<GenericModel>("sp_ProfileChangeStatus", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        #endregion
        #region menus
        public async Task<GenericModel> ProfileAcessmenusSynchronizeAsync(int ProfileCode, int Makerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProfileCode", ProfileCode);
                parameters.Add("@CreatedBy", Makerid);
                return (await connection.QueryAsync<GenericModel>("sp_ProfileAccessMenuSync", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        #endregion
        #region Audit Trail
        public async Task<IEnumerable<ViewAuditTrail>> GetAuditTrailsAsync(AuditFilterModel req)
        {
            return await Task.Run(() =>
            {
                var resp = new List<ViewAuditTrail>();
                try
                {
                    using (var connection = new SqlConnection(_connString))
                    {
                        connection.Open();
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@MakerId", req.MakerId);
                        parameters.Add("@ModuleId", req.ModuleId);
                        parameters.Add("@DateFrom", req.DateFrom);
                        parameters.Add("@DateTo", req.DateTo);
                        if (req.ModuleId == 1)//Admmin
                        {
                            parameters.Add("@UserCode", req.UserCode);
                        }
                        else//Client
                        {
                            parameters.Add("@ClientCode", req.ClientCode);
                            parameters.Add("@OperatorCode", req.OperatorCode);
                        }
                        return connection.Query<ViewAuditTrail>("Sp_AuditTrails", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                catch (Exception ex)
                {
                    resp.Add(new ViewAuditTrail { RespStatus = 3, RespMessage = ex.Message });
                }
                return resp;
            });
        }
        public async Task<BaseEntity> CreateAuditActionAsync(UserAudit data)
        {
            var resp = new BaseEntity();
            return await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(_connString))
                    {
                        connection.Open();
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@UsrCode", data.UserCode);
                        parameters.Add("@ActDescr", data.ActionDescr);
                        parameters.Add("@Modid", data.ModuleId);
                        parameters.Add("@ModFunc", data.MdlFunction);
                        parameters.Add("@ClntIP", data.ClntIP);
                        parameters.Add("@location", data.Location_);
                        parameters.Add("@Browser", data.Browser);
                        resp = connection.Query<BaseEntity>("sp_AuditAdd", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    resp.RespStatus = 3;
                    resp.RespMessage = ex.Message;
                }
                return resp;
            });
        }

        #endregion
    }
}
