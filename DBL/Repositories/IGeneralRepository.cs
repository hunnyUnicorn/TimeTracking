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
        Task<GenericModel> UpdateVCodes(string useridentifier, int usertype);
        Task<IEnumerable<Notification>> GetNotifications(int custcode, USER_TYPE usertype);
        Task<GenericModel> GenerateResetLink(PasswodResetModel model);
        Task<BaseEntity> ValidateLink(string id);
        Task<BaseEntity> ResetPassword(PasswodResetModel model);
    }
}
