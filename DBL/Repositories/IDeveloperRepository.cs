using DBL.Entities;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IDeveloperRepository
    {
        Task<GenericModel> RegisterDeveloper(Developer model);
        Task<GenericModel> VerifyDeveloper(string email);
        Task<IEnumerable<GenericModel>> DeveloperLogin(int developerCode, int status);
        Task<IEnumerable<Project>> GetAssignedProjects(int devcode);
        Task<BaseEntity> RecordScreenshot(Screenshot model);
        Task<IEnumerable<screenshotdets>> GetScreenShots(int filterType, string value, int clientcode);
        Task<GenericModel> CreateTimeFrame(TimeTrack model);
        Task<BaseEntity> StopTimeFrame(int TTCode, int KeyHits, int mouseHits);
    }
}
