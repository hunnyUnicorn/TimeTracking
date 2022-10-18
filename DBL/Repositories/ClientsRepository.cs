using Dapper;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBL.Entities;

namespace DBL.Repositories
{
    public class ClientsRepository : BaseRepository, IClientsRepository
    {
        public ClientsRepository(string connectionString) : base(connectionString)
        {

        }
        public async Task<GenericModel> RegisterClient(Client model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientName", model.ClientName);
                parameters.Add("@email", model.email);
                parameters.Add("@Salt", model.Salt);
                parameters.Add("@Pwd", model.Pwd);
                parameters.Add("@UserIdentifier", model.UserIdentifier);
                return (await connection.QueryAsync<GenericModel>("Sp_Create_Client", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> VerifyClient(string email)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                return (await connection.QueryAsync<GenericModel>("Sp_Verify_Client", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<GenericModel>> ClientLogin(int clientcode, int status)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientCode", clientcode);
                parameters.Add("@LoginStat", status);
                return (await connection.QueryAsync<GenericModel>("sp_Client_Login", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<Client>> GetClients()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return (await connection.QueryAsync<Client>("Sp_Get_Clients", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        #region Projects
        public async Task<BaseEntity> CreateProject(Project model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientCode", model.ClientCode);
                parameters.Add("@ProjectName", model.ProjectName);
                parameters.Add("@ProjectDescr", model.ProjectDescr);
                parameters.Add("@ProjCat", model.ProjCatCode);
                parameters.Add("@ProjRef", model.ProjRef);
                parameters.Add("@EndDate", model.ProjEndDate);
                parameters.Add("@CCYCode", model.CCYCode);
                return (await connection.QueryAsync<BaseEntity>("Sp_Create_Project", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<Project>> Projects(int clientCode, int status)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientCode", clientCode);
                parameters.Add("@Status", status);
                return (await connection.QueryAsync<Project>("Sp_Pull_Projects", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<GenericModel> InviteDeveloper(ProjectInviteModel model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientCode", model.ClientCode);
                parameters.Add("@ProjectCode", model.ProjectCode);
                parameters.Add("@Email", model.Email);
                return (await connection.QueryAsync<GenericModel>("Sp_Add_Developer_To_Project", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<Project> GetProject(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from projects where ProjectCode=" + code + "";
                connection.Open();
                return (await connection.QueryAsync<Project>(sql)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<Developer>> DevelopersPerProject(int projectcode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@projectcode", projectcode);
                return (await connection.QueryAsync<Developer>("Sp_Developer_Projects_List", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<screenshotdets>> GetScreenShotsPerClient(int filterType,string value,int clientcode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Filter", filterType);
                parameters.Add("@Value", value);
                parameters.Add("@ClientCode", clientcode);
                return (await connection.QueryAsync<screenshotdets>("Sp_ScreenCasts_Client", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<TimeTrack>> TimeTracks(int clientCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientCode", clientCode);
                return (await connection.QueryAsync<TimeTrack>("Sp_GetTimeTracks_Client", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        #endregion
    }
}
