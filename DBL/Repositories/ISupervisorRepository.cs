using DBL.Enums;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface ISupervisorRepository
    {
        Task<IEnumerable<MCActionRawData>> GetMCActionRawDataAsync(int actionCode, int userCode);
        Task<IEnumerable<SupervisorQueueModel>> GetMySupervisorQueueAsync(int userCode, int type, McCategory category, int group);
        Task<GenericModel> TakeMCActionAsync(int actionCode, int userCode, int action, string reason);
    }
}
