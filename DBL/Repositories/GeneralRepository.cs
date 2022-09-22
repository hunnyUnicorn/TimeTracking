﻿using Dapper;
using DBL.Enums;
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
    public class GeneralRepository:BaseRepository,IGeneralRepository
    {
        public GeneralRepository(string connectionString) : base(connectionString)
        {
        }
        public async Task<List<ListModel>> GetItemListAsync(ListItemType itemType, int code = 0)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Type", (int)itemType);
                parameters.Add("@Code", code);

                return (await connection.QueryAsync<ListModel>("sp_GetListModel", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<SysSetting> SysSetting(string name)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                var sql = "Select ItemName,ItemValue From DBYDUsers where ItemName='" + name+"'";

                return (await connection.QueryAsync<SysSetting>(sql)).FirstOrDefault();
            }
        }
        public async Task<int> GetUserType(string email)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@email", email);

                return (await connection.QueryAsync<int>("Sp_Verify_User_Type", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> CreateVCode(VCodeModel model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RawVCode", model.RawVCode);
                parameters.Add("@VCode", model.VCode);
                parameters.Add("@Salt", model.Salt);
                parameters.Add("@usertype", model.UserType);
                parameters.Add("@useridentifier", model.useridentifier);

                return (await connection.QueryAsync<GenericModel>("Sp_Create_VerificationCode", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> GetVCodeDetails(string useridentifier)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@userIdentifier", useridentifier);

                return (await connection.QueryAsync<GenericModel>("Sp_Verify_VCode", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
    }
}
