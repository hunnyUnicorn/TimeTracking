using Dapper;
using DBL.Entities;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class DeveloperRepository : BaseRepository, IDeveloperRepository
    {
        public DeveloperRepository(string connectionString) : base(connectionString)
        {

        }
        public async Task<GenericModel> RegisterDeveloper(Developer model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FirstName", model.FirstName);
                parameters.Add("@MiddleName", model.MiddleName);
                parameters.Add("@Surname", model.Surname);
                parameters.Add("@email", model.email);
                parameters.Add("@Salt", model.Salt);
                parameters.Add("@Pwd", model.Pwd);
                parameters.Add("@userIdentifier", model.UserIdentifier);
                return (await connection.QueryAsync<GenericModel>("Sp_Create_Developer", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> VerifyDeveloper(string email)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                return (await connection.QueryAsync<GenericModel>("Sp_Verify_Developer", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<GenericModel>> DeveloperLogin(int developerCode, int status)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DevCode", developerCode);
                parameters.Add("@LoginStat", status);
                return (await connection.QueryAsync<GenericModel>("sp_Developer_Login", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<Project>> GetAssignedProjects(int devcode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DevCode", devcode);
                return (await connection.QueryAsync<Project>("Sp_Get_Projects_For_Developer", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<BaseEntity> RecordScreenshot(Screenshot model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ScrName", model.ScrName);
                parameters.Add("@DevCode", model.DevCode);
                parameters.Add("@ProjCode", model.ProjCode);
                return (await connection.QueryAsync<BaseEntity>("Sp_Screenshot", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<screenshotdets>> GetScreenShots(int filterType, string value, int clientcode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Filter", filterType);
                parameters.Add("@Value", value);
                parameters.Add("@DevCode", clientcode);
                return (await connection.QueryAsync<screenshotdets>("Sp_ScreenCasts_Developer", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<BaseEntity> CreateTimeFrame(Screenshot model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TTDescr", model.ScrName);
                parameters.Add("@DevCode", model.DevCode);
                parameters.Add("@ProjCode", model.ProjCode);
                return (await connection.QueryAsync<BaseEntity>("Sp_Add_TimeTrack", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
    }
}
