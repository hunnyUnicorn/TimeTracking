using DBL.Enums;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IGeneralRepository
    {
        Task<List<ListModel>> GetItemListAsync(ListItemType itemType, int code = 0);
        Task<SysSetting> SysSetting(string name);
        Task<int> GetUserType(string email);
        Task<GenericModel> CreateVCode(VCodeModel model);
        Task<GenericModel> GetVCodeDetails(string useridentifier);
    }
}
