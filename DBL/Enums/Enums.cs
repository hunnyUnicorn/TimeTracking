using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Enums
{
    public enum UserLoginStatus { Ok = 0, ChangePassword = 1, PassExpired = 2, UserLocked = 3, AttemptsExceeded = 4 }

    public enum CBSSettType { AccountUrl = 0, PostingUrl = 1, FlexDb = 2, MiniStatement = 4, LoanLimit = 5, LoanBalance = 6, PostLoan = 7, RepayLoan = 8 }

    public enum ListItemType
    {
        ProjectDevelopers = 0,
        ProjectCategories = 1,
        Currencies = 2,
        DeveloperProjects = 3
    }

    public enum ReportsMode
    {
        Status = 0,
        Type = 2,
    }

    public enum AdminData
    {
        Users = 0,
        UserGroup = 1,
        Profile = 2,
        comission = 3,
    }

    public enum AccessRights
    {
        Grant = 1,
        Revoke = 2,
    }

    public enum Group
    {
        Profiles = 10,
        Users = 11
    }

    public enum McCategory
    {
        Supervisor = 0,
        Security = 1,
        Transactions =2
    }

    public enum QueueActionTypes { Create = 0, Modify = 1, Remove = 2, AddCustService = 3 }

    public enum QueueItemTypes { Customer = 1, Service = 2 }

    public enum InquiryTypes { Balance = 0, MiniStatement = 1 }
    public enum AuditStat
    {
        Created = 1,
        Modified = 2,
        Deleted = 3,
        Viewed = 4,
        Approved = 5,
        Rejected = 6,
        Activated = 7,
        Blocked = 8,
        ResetPassword = 9,
        LoggedIn = 10,
        LoggedOut = 11,
        VerifiedOTP = 12,
        ResendOPT = 13,
        Downloaded = 14,
    }
    public enum USER_TYPE
    {
        DEVELOPER =1,
        CLIENT =2
    }
}
