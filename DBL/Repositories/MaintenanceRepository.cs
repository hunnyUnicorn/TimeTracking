using Dapper;
using DBL.Entities;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class MaintenanceRepository : BaseRepository, IMaintenanceRepository
    {
        public MaintenanceRepository(string connectionString) : base(connectionString)
        {
        }
        public async Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from SubscriptionPlans";
                connection.Open();
                return (await connection.QueryAsync<Subscription>(sql)).ToList();
            }
        }
        public async Task<IEnumerable<ProjectCategory>> GetProjectCategories()
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from ProjectCats";
                connection.Open();
                return (await connection.QueryAsync<ProjectCategory>(sql)).ToList();
            }
        }
        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            using (var connection = new SqlConnection(_connString))
            {
                string sql = "select * from currency";
                connection.Open();
                return (await connection.QueryAsync<Currency>(sql)).ToList();
            }
        }
        public async Task<BaseEntity> CreateCurrency(Currency model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CCYName", model.CCYName);
                parameters.Add("@ISOName", model.ISOName);
                parameters.Add("@CCYSymbol", model.CCYSymbol);
                return (await connection.QueryAsync<BaseEntity>("Sp_Create_Currency", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
    }
}
