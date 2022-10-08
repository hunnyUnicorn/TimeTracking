using DBL.Models;
using DBL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IMaintenanceRepository
    {
        Task<IEnumerable<Subscription>> GetSubscriptions();
        Task<IEnumerable<ProjectCategory>> GetProjectCategories();
    }
}
