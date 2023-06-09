﻿using DBL.Entities;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IClientsRepository
    {
        Task<GenericModel> RegisterClient(Client model);
        Task<GenericModel> VerifyClient(string email);
        Task<IEnumerable<GenericModel>> ClientLogin(int clientcode, int status);
        Task<IEnumerable<Client>> GetClients();
        #region projects
        Task<BaseEntity> CreateProject(Project model);
        Task<IEnumerable<Project>> Projects(int clientCode, int status);
        Task<GenericModel> InviteDeveloper(ProjectInviteModel model);
        Task<Project> GetProject(int code);
        Task<IEnumerable<Developer>> DevelopersPerProject(int projectcode);
        Task<IEnumerable<screenshotdets>> GetScreenShotsPerClient(int filterType, string value, int clientcode);
        Task<IEnumerable<TimeTrack>> TimeTracks(int clientCode);
        #endregion
        #region Invoices
        Task<IEnumerable<TimeTrackerInvoice>> GetClientInvoices(int ClientCode);
        #endregion
        Task<SubPlanDets> GetPlanDetails(int clientcode);
    }
}
