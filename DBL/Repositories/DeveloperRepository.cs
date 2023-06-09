﻿using Dapper;
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
                parameters.Add("@TTCode", model.TTCode);
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
        public async Task<GenericModel> CreateTimeFrame(TimeTrack model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TTDescr", model.TTDescr);
                parameters.Add("@DevCode", model.DevCode);
                parameters.Add("@ProjCode", model.ProjectCode);
                parameters.Add("@StartDate", model.BeginDate);
                return (await connection.QueryAsync<GenericModel>("Sp_Add_TimeTrack", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<BaseEntity> StopTimeFrame(int TTCode,int KeyHits,int mouseHits,DateTime closeDate)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TimeTrackCode", TTCode);
                parameters.Add("@KeyboardHits", KeyHits);
                parameters.Add("@MouseClicks", mouseHits);
                parameters.Add("@CloseDate", closeDate);
                return (await connection.QueryAsync<BaseEntity>("Sp_CloseTimeTrack", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<TimeTrack>> GetTimeTracks(int developercode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DevCode", developercode);
                return (await connection.QueryAsync<TimeTrack>("Sp_GetTimeTracks", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<ProjectInvite> ProjectInvite(int InviteCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InviteCode", InviteCode);
                return (await connection.QueryAsync<ProjectInvite>("Sp_Project_Invite", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<ProjectInvite>> ProjectInvites(int DeveloperCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DevCode", DeveloperCode);
                return (await connection.QueryAsync<ProjectInvite>("Sp_Project_Invites", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<BaseEntity> Invite_Action(int InviteAction, int DevCode, int InviteCode,string RejectReason)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InviteAction", InviteAction);
                parameters.Add("@DevCode", DevCode);
                parameters.Add("@InviteCode", InviteCode);
                parameters.Add("@RejectReason", RejectReason);
                return (await connection.QueryAsync<BaseEntity>("Sp_Invite_Action", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<IEnumerable<Developer>> GetDevelopersAsync()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return (await connection.QueryAsync<Developer>("Sp_Get_Developers", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<TimeTrackerInvoice>> GetDeveloperInvoices(int DevCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DevCode", DevCode);
                return (await connection.QueryAsync<TimeTrackerInvoice>("Sp_GetInvoices_Developer", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<IEnumerable<InvoiceDets>> GetInvoiceDets(int invoicecode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InvoiceCode", invoicecode);
                return (await connection.QueryAsync<InvoiceDets>("Sp_Get_InvoiceDets", parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }
        public async Task<BaseEntity> Create_Invoice(TimeTrackerInvoice model)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@InvoiceType", model.InvoiceType);
                parameters.Add("@StartDate", model.StartDate);
                parameters.Add("@EndDate", model.EndDate);
                parameters.Add("@ProjectCode", model.ProjectCode);
                parameters.Add("@DeveloperCode", model.DeveloperCode);
                return (await connection.QueryAsync<BaseEntity>("Sp_Create_Invoice", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<GenericModel> Get_Client_Invoice_Mail_Settings(int invoicecode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@invoicecode", invoicecode);
                return (await connection.QueryAsync<GenericModel>("Sp_Client_Mail_Settings", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<StripeDets> CreateTxn(int subplancode,int clientcode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@SubPlanCode", subplancode);
                parameters.Add("@ClientCode", clientcode);
                return (await connection.QueryAsync<StripeDets>("Sp_Renew_Subscription", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<BaseEntity> UpdateStripeDets(int txncode, string stripesessionid,string paymentintentid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TxnCode", txncode);
                parameters.Add("@StripeSession", stripesessionid);
                parameters.Add("@StripePaymentIntentId", paymentintentid);
                return (await connection.QueryAsync<BaseEntity>("Sp_UpdateTxnSession", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
        public async Task<SubPlanDets> GetPlanDetails(int devcode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ClientCode", devcode);
                parameters.Add("@usertype", 1);
                return (await connection.QueryAsync<SubPlanDets>("Sp_Get_SubDets", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }
    }
}
