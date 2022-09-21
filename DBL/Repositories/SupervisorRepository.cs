using Dapper;
using DBL.Enums;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class SupervisorRepository: BaseRepository,ISupervisorRepository
    {
        public SupervisorRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<MCActionRawData>> GetMCActionRawDataAsync(int actionCode, int userCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AtionCode", actionCode);
                parameters.Add("@UserCode", userCode);

                return (await connection.QueryAsync<MCActionRawData>("sp_GetMCActionData", parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<IEnumerable<SupervisorQueueModel>> GetMySupervisorQueueAsync(int userCode, int type, McCategory category, int group)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserCode", userCode);
                parameters.Add("@Role", type);
                parameters.Add("@Cat", category);
                parameters.Add("@GroupCode", group);

                return (await connection.QueryAsync<SupervisorQueueModel>("sp_GetMySupervisorQueue", parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<GenericModel> TakeMCActionAsync(int actionCode, int userCode, int action, string reason)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ActionCode", actionCode);
                parameters.Add("@UserCode", userCode);
                parameters.Add("@MCAction", action);
                parameters.Add("@Reason", reason);

                return (await connection.QueryAsync<GenericModel>("sp_MCTakeAction", parameters, commandType: System.Data.CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
    }
}
