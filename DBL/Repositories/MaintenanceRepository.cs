using Dapper;
using DBL.Entities;
using DBL.Models;
using System;
using System.Collections.Generic;
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
    }
}
