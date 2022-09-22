using BMobile.DBL.Models;
using DBL.Entities;
using DBL.Models;
using DBL.UOW;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Taiwac.Tools;
using DBL.Consts;
using DBL.Enums;
using System.Net.Mail;
using System.Net;

namespace DBL
{
    public class Bl
    {
        private UnitOfWork db;
        private string _connString;
        public string LogFile { get; set; }

        public Bl(string connString)
        {
            this._connString = connString;
            db = new UnitOfWork(connString);
        }
        #region User
        public async Task<UserModel> UserLoginAsync(string userName, string password)
        {
            UserModel resp = new UserModel { };
            //------ Get User
            var user = await db.SecurityRepository.VerifyAsync(userName);
            if (user == null)
            {
                resp.RespStatus = 1;
                resp.RespMessage = "Invalid Username and/or Password!";
                return resp;
            }

            //----- Check user status
            if (user.RespStatus == 0)
            {
                //----- Validate User Password
                var sec = new TSSecurity();
                if (sec.ValidatePassword(password, user.Data2, user.Data3))
                {
                    var respData = await db.SecurityRepository.UserLoginAsync(Convert.ToInt32(user.Data1), 0);
                    db.Reset();
                    if (respData == null)
                    {
                        resp.RespStatus = 1;
                        resp.RespMessage = "Technical Error- Login Aborted";
                        return resp;
                    }
                    var CheckSuccessData = respData.Where(x => x.ItemType == 1).FirstOrDefault();
                    if (int.Parse(CheckSuccessData.Data2) == 0)
                    {
                        var CheckUserData = respData.Where(x => x.ItemType == 2).FirstOrDefault();
                        if (int.Parse(CheckUserData.Data2) < 1)
                        {
                            resp.RespStatus = 1;
                            resp.RespMessage = "Technical Error- Loading User Details";
                            return resp;
                        }
                        else
                        {
                            resp = new UserModel();
                            resp.FullNames = CheckUserData.Data3.Trim();
                            resp.ChangePass = CheckUserData.Data5.Trim() == "1" ? true : false;
                            resp.UserCode = int.Parse(CheckUserData.Data2.Trim());
                            resp.ProfileCode = int.Parse(CheckUserData.Data6.Trim());
                            resp.Extra1 = "";
                            resp.ProfileAccessData = respData.Where(x => x.ItemType == 3).Select(x => new Viewmenus
                            {
                                MenuCode = int.Parse(x.Data2),
                                MenuName = x.Data1,
                                ParentId = int.Parse(x.Data3),
                                MenuLevel = int.Parse(x.Data4),
                                GroupCode = int.Parse(x.Data9),
                                Controller = x.Data5,
                                ActionName = x.Data6,
                                IconName = x.Data7,
                                OrderBy = x.Data8
                            }).ToList();
                            return resp;
                        }
                    }
                    else
                    {
                        resp.RespStatus = 1;
                        resp.RespMessage = CheckSuccessData.Data3;
                    }
                }
                else
                {
                    var respData = await db.SecurityRepository.UserLoginAsync(Convert.ToInt32(user.Data1), 1);
                    db.Reset();

                    resp.RespStatus = 1;
                    resp.RespMessage = "Incorrect Username and/or Password!";
                }
            }
            else
            {
                resp.RespStatus = 1;
                resp.RespMessage = user.RespMessage;
            }
            resp.ProfileAccessData = new List<Viewmenus>();
            return resp;
        }


        public async Task<IEnumerable<SysUserModel>> GetUsersAllAsync()
        {
            var user = await db.SecurityRepository.GetUsersAllAsync();
            return user;
        }

        public async Task<IEnumerable<SysUserModel>> GetUsersAsync(int profile, string query = "")
        {
            var users = await db.SecurityRepository.GetUsersAsync(profile, query);
            return users;
        }

        public async Task<SysUserModel> GetUserAsync(int code)
        {
            var user = await db.SecurityRepository.GetUserAsync(code);
            return user;
        }
        public async Task<User> GetUser(int code)
        {
            var user = await db.SecurityRepository.GetUser(code);
            return user;
        }

        public async Task<BaseEntity> CreateUserAsync(User user)
        {
            TSSecurity security = new TSSecurity();
            string salt = security.GenerateSalt(25);
            string rawPass = Util.GenerateSimplePassword();
            string password = security.HashPassword(rawPass, salt);

            user.Salt = salt;
            user.Pwd = password;

            var result = await db.SecurityRepository.CreateUserAsync(user, rawPass);
            db.Reset();

            return new BaseEntity
            {
                RespMessage = result.RespMessage,
                RespStatus = result.RespStatus
            };
        }
        public async Task<BaseEntity> ModifyUserAsync(User user)
        {
            var result = await db.SecurityRepository.ModifyUserAsync(user);
            db.Reset();

            return new BaseEntity
            {
                RespMessage = result.RespMessage,
                RespStatus = result.RespStatus
            };
        }
        public async Task<BaseEntity> ChangeUserPasswordAsync(ChangeUserPassModel data)
        {
            //---- Get user
            var passDets = await db.SecurityRepository.GetUserPasswordAsync(data.UserCode);
            if (passDets.RespStatus != 0)
            {
                return new BaseEntity
                {
                    RespStatus = passDets.RespStatus,
                    RespMessage = passDets.RespMessage
                };
            }

            //---- Validate old password
            TSSecurity sec = new TSSecurity();
            if (!sec.ValidatePassword(data.OldPassword, passDets.Data1, passDets.Data2))
            {
                return new BaseEntity
                {
                    RespStatus = 1,
                    RespMessage = "Invalid old password!"
                };
            }

            //----- Create and update password
            string salt = sec.GenerateSalt(30);
            string password = sec.HashPassword(data.NewPassword, salt);

            var result = await db.SecurityRepository.ChangeUserPasswordAsync(data.UserCode, password, salt, data.NewPassword);
            db.Reset();

            return result;
        }

        public async Task<BaseEntity> MakeUserActionAsync(int userCode, int action, int createdBy)
        {
            if (action == 1)
            {
                string newPassword = Util.GenerateSimplePassword();
                TSSecurity security = new TSSecurity();
                string salt = security.GenerateSalt(25);
                string password = security.HashPassword(newPassword, salt);

                var result = await db.SecurityRepository.ResetUserPasswordAsync(userCode, salt, password, newPassword, createdBy);
                db.Reset();
                return result;
            }
            else
            {
                int status = action == 2 ? 3 : 1;
                var result = await db.SecurityRepository.ChangeUserStatusAsync(userCode, status, createdBy);
                db.Reset();
                return result;
            }
        }


        #endregion

        #region Profiles 
        public async Task<GenericModel> CreateProfilesAsync(Profile group)
        {
            return await db.SecurityRepository.CreateProfileAsync(group);
        }
        public async Task<GenericModel> ModifyProfileAsync(Profile group)
        {
            return await db.SecurityRepository.ModifyProfileAsync(group);
        }
        public async Task<ProfileAccess> GetProfileAccessViewAsync(int ProfileAccessCode)
        {
            return await db.SecurityRepository.GetProfileAccessViewAsync(ProfileAccessCode);
        }
        public async Task<GenericModel> ProfileCreateAccessAsync(int pacode, int CreatedBy, int stat)
        {

            return await db.SecurityRepository.ProfileCreateAccess(pacode, CreatedBy, stat);
        }
        public async Task<Profile> GetProfileAsync(int code)
        {
            return await db.SecurityRepository.GetProfileAsync(code);
        }
        public async Task<IEnumerable<Profile>> GetProfilesAsync(string query)
        {
            return await db.SecurityRepository.GetProfileListAsync(query);
        }
        public async Task<IEnumerable<Viewmenus>> GetProfileMenus(int ProfileCode)
        {
            return await db.SecurityRepository.GetProfileAccess(ProfileCode);
        }
        public async Task<IEnumerable<ProfileAccess>> GetProfileAccessListAsync(int ProfileCode)
        {
            return await db.SecurityRepository.GetProfileAccessListAsync(ProfileCode);
        }
        public async Task<GenericModel> MakeProfileActionAsync(int ProfileCode, int action, int createdBy)
        {
            if (action == 1)
                return new GenericModel { RespStatus = 3, RespMessage = "Action Not Implemented" };
            else
            {
                int status = action == 2 ? 3 : 1;
                var result = await db.SecurityRepository.ChangeProfileStatusAsync(ProfileCode, status, createdBy);
                db.Reset();
                return result;
            }
        }
        #endregion

        #region Menus
        //ProfileAcessmenusSynchronizeAsync
        public async Task<GenericModel> ProfileAcessmenusSynchronizeAsync(int ProfileCode, int createdBy)
        {
            var result = await db.SecurityRepository.ProfileAcessmenusSynchronizeAsync(ProfileCode, createdBy);
            db.Reset();
            return result;
        }
        #endregion
        #region Audit Trail        
        public async Task<BaseEntity> CreateAuditActionAsync(string Browser, string ActionDescr, int moduleid, string MdlFunction, int UserCode, string ClntIP, string ClntLocation)
        {
            try
            {
                var data = new UserAudit
                {
                    Id = 0,
                    ActionDescr = ActionDescr,
                    AuditDate = DateTime.Now,
                    Browser = Browser,
                    ClntIP = ClntIP,
                    Location_ = ClntLocation,
                    MdlFunction = MdlFunction,
                    ModuleId = moduleid,
                    UserCode = UserCode
                };
                return await db.SecurityRepository.CreateAuditActionAsync(data);
            }
            catch (Exception ex)
            {
                return new BaseEntity { RespStatus = 3, RespMessage = ex.Message };
            }
        }

        public async Task<IEnumerable<ViewAuditTrail>> GetAuditTrailsAsync(AuditFilterModel data)
        {
            return await db.SecurityRepository.GetAuditTrailsAsync(data);
        }

        #endregion
        #region DEVELOPER REQUERSTS
        public async Task<ApiResponseData> ProcessDeveloperRequests(ApiRequestData requestData)
        {
            var resp = new ApiResponseData { RespStatus = 1, Message = "Request received but was not processed!" };

            var contentData =requestData.Content.ToString();
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(contentData);
            switch (requestData.Action)
            {
                case DEVELOPER_ACTIONS.SIGNUP:
                    #region ADD DEVELOPER
                    var devModel = JsonConvert.DeserializeObject<Developer>(contentData);

                    if (devModel != null)
                    {
                        TSSecurity security = new TSSecurity();
                        string salt = security.GenerateSalt(25);
                        string rawPass =devModel.RawPass;
                        string password = security.HashPassword(rawPass, salt);

                        devModel.Salt = salt;
                        devModel.Pwd = password;
                        var result = await db.DeveloperRepository.RegisterDeveloper(devModel);
                        db.Reset();

                        if (result.RespStatus == 0)
                        {
                            return new ApiResponseData
                            {
                                RespStatus = result.RespStatus,
                                Message = result.RespMessage
                            };
                        }
                        else
                        {
                            if (result.RespStatus == 1)
                            {
                                resp.Message = result.RespMessage;
                                resp.MessageCode = result.Data1;
                            }
                            else
                                throw new Exception(result.RespMessage);
                        }
                    }
                    #endregion
                    break;
                case DEVELOPER_ACTIONS.LOGIN:
                    #region LOGIN DEVELOPER
                        var devLogin = JsonConvert.DeserializeObject<UserLoginModel>(contentData);
                        var loginResp = await VerifyDeveloper(devLogin);
                        resp.Data = loginResp;
                        resp.RespStatus = loginResp.RespStatus;
                        resp.Message = loginResp.RespMessage;
                    #endregion
                    break;
                case DEVELOPER_ACTIONS.GET_PROJECTS:
                    #region GET PROJECTS
                        var projects = await db.DeveloperRepository.GetAssignedProjects(Convert.ToInt32(contentData));
                        resp.Data = projects;
                        resp.RespStatus = 0;
                        resp.Message = "Assigned Projects";
                    #endregion
                    break;
                case DEVELOPER_ACTIONS.POST_SCREENSHOT:
                    #region POST SCREENSHOT
                    var screenshot = new Screenshot {
                        base64String = dynamicObject.base64String,
                        DevCode = dynamicObject.DevCode,
                        ProjCode = dynamicObject.ProjCode
                    };
;
                    screenshot.ScrName = Guid.NewGuid().ToString();
                    screenshot.base64String = screenshot.base64String.Substring(screenshot.base64String.LastIndexOf(',') + 1);
                    string filepath =Path.Combine(@"E:\TimeTrackerImages", screenshot.ScrName+".png") ;
                    Util.SaveImage(screenshot.base64String, filepath);
                    var screenshotResp = await db.DeveloperRepository.RecordScreenshot(screenshot);
                    resp.RespStatus = screenshotResp.RespStatus;
                    resp.Message = screenshotResp.RespMessage;
                    #endregion
                    break;
            }

            return resp;
        }
        public async Task<Developer> VerifyDeveloper(UserLoginModel model)
        {
            Developer resp = new Developer { };
            var developer = await db.DeveloperRepository.VerifyDeveloper(model.Username);
            if (developer == null)
            {
                resp.RespStatus = 1;
                resp.RespMessage = "Invalid Username and/or Password!";
                return resp;
            }

            //----- Check user status
            if (developer.RespStatus == 0)
            {
                //----- Validate User Password
                var sec = new TSSecurity();
                if (sec.ValidatePassword(model.Password, developer.Data2, developer.Data3))
                {
                    var respData = await db.DeveloperRepository.DeveloperLogin(Convert.ToInt32(developer.Data1), 0);
                    db.Reset();
                    if (respData == null)
                    {
                        resp.RespStatus = 1;
                        resp.RespMessage = "Technical Error- Login Aborted";
                        return resp;
                    }
                    var CheckSuccessData = respData.Where(x => x.ItemType == 1).FirstOrDefault();
                    if (int.Parse(CheckSuccessData.Data2) == 0)
                    {
                        var CheckUserData = respData.Where(x => x.ItemType == 2).FirstOrDefault();
                        if (int.Parse(CheckUserData.Data2) < 1)
                        {
                            resp.RespStatus = 1;
                            resp.RespMessage = "Technical Error- Loading User Details";
                            return resp;
                        }
                        else
                        {
                            resp = new Developer();
                            resp.FirstName = CheckUserData.Data3.Trim();
                            resp.MiddleName = CheckUserData.Data4.Trim();
                            resp.Surname = CheckUserData.Data5.Trim();
                            resp.email = CheckUserData.Data8.Trim();
                            resp.DevCode =Convert.ToInt32(CheckUserData.Data2.Trim());
                            resp.RespMessage = CheckSuccessData.Data3;
                            return resp;
                        }
                    }
                    else
                    {
                        resp.RespStatus = 1;
                        resp.RespMessage = CheckSuccessData.Data3;
                    }
                }
                else
                {
                    var respData = await db.SecurityRepository.UserLoginAsync(Convert.ToInt32(developer.Data1), 1);
                    db.Reset();

                    resp.RespStatus = 1;
                    resp.RespMessage = "Incorrect Username and/or Password!";
                }
            }
            else
            {
                resp.RespStatus = 1;
                resp.RespMessage = developer.RespMessage;
            }
            return resp;
        }
       
        #endregion
        #region Developer API Actions
        public async Task<GenericModel> DeveloperSignUp(Developer model)
        {
            string url = "http://localhost:5232/developers/api/v1/ProcessRequest";

            using (var httpClient = new RestApiClient(url, RestApiClient.RequestType.Post))
            {
                ApiRequestData reqData = new ApiRequestData {Content=JsonConvert.SerializeObject(model),Action=100 };
                var postData = JsonConvert.SerializeObject(reqData);
                var result = await httpClient.SendRequestAsync(postData);
                if (result.Success)
                {
                    var httpResult = JsonConvert.DeserializeObject<ApiResponseData>(result.Data);
                    return new GenericModel {RespStatus = httpResult.RespStatus,RespMessage=httpResult.Message };
                }
                else
                {
                    if (result.Exception != null)
                        LogUtil.Error(LogFile, "Bl.ServiceCallAsync()", result.Exception);

                    string respMsg = "";
                    if (!string.IsNullOrEmpty(result.Exception.Message))
                    {
                        respMsg = result.Exception.Message;
                        LogUtil.Error(LogFile, "Bl.ServiceCallAsync()", result.Exception);
                    }
                    else if (!string.IsNullOrEmpty(result.Data))
                    {
                        respMsg = result.Data;
                        LogUtil.Error(LogFile, "Bl.ServiceCallAsync()", result.Data);
                    }

                    return new GenericModel { RespStatus = 1, RespMessage = respMsg };
                }
            }
        }
        #endregion
        #region Developer Actions
        public async Task<GenericModel> CreateDeveloper(Developer model)
        {
            TSSecurity security = new TSSecurity();
            string salt = security.GenerateSalt(25);
            string rawPass = model.RawPass;
            string password = security.HashPassword(rawPass, salt);

            model.Salt = salt;
            model.Pwd = password;
            model.UserIdentifier = Guid.NewGuid().ToString();
            var result = await db.DeveloperRepository.RegisterDeveloper(model);
            result.Data1 = model.UserIdentifier;
            if(result.RespStatus==0)
            {
                //Generate Verification Code
                VCodeModel vCode = new VCodeModel();
                string verificationCode = Util.GenerateSimplePassword();
                vCode.RawVCode = verificationCode;
                vCode.Salt = security.GenerateSalt();
                vCode.VCode = security.HashPassword(verificationCode, vCode.Salt);
                vCode.UserType = USER_TYPE.DEVELOPER;
                vCode.useridentifier = model.UserIdentifier;
                try
                {
                    var vcodeResp = await db.GeneralRepository.CreateVCode(vCode);
                    if (vcodeResp.RespStatus == 0)
                    {
                        int port = Convert.ToInt32(vcodeResp.Data5);
                        SendMail(vcodeResp.Data4, port, vcodeResp.Data6 == "1", vcodeResp.Data7, vcodeResp.Data8, vcodeResp.Data3, vcodeResp.Data9, vcodeResp.Data2);
                    }
                    else
                    {
                        result.RespStatus = 1;
                        result.RespMessage = "Error occured when executing your request";
                        LogUtil.Error(LogFile, "Generate VCode", new Exception(vcodeResp.RespMessage));
                    }
                }
                catch(Exception ex)
                {
                    LogUtil.Error(LogFile, "Generate VCode", ex);
                    result.RespStatus = 1;
                    result.RespMessage = "Error occured when executing your request";
                }
            }
            return result;
        }

        #endregion
        #region Common Actions
        public async Task<int> Login(UserLoginModel model)
        {
            var usertype = await db.GeneralRepository.GetUserType(model.Username);
            return usertype;
        }
        public async Task<BaseEntity> VerifyEmail(EmailVerification model)
        {
            var vcodedetails = await db.GeneralRepository.GetVCodeDetails(model.userIdentifier);
            if(vcodedetails.RespStatus==0)
            {
                var sec = new TSSecurity();
                if (sec.ValidatePassword(model.VerifcationCode, vcodedetails.Data1, vcodedetails.Data2))
                {
                    return new BaseEntity { 
                        RespMessage="success",
                        RespStatus = 0
                    };
                }
                else
                {
                    return new BaseEntity
                    {
                        RespMessage = "Invalid verification code",
                        RespStatus = 1
                    };
                }
            }
            else
            {
                return new BaseEntity
                {
                    RespStatus = vcodedetails.RespStatus,
                    RespMessage = vcodedetails.RespMessage
                };
            }
        }
        public async Task<IEnumerable<ListModel>> GetItemListAsync(ListItemType itemType, int code = 0)
        {
            return await db.GeneralRepository.GetItemListAsync(itemType, code);
        }
        #endregion
        #region Client Actions
        public async Task<Client> VerifyClient(UserLoginModel model)
        {
            Client resp = new Client { };
            var developer = await db.ClientsRepository.VerifyClient(model.Username);
            if (developer == null)
            {
                resp.RespStatus = 1;
                resp.RespMessage = "Invalid Username and/or Password!";
                return resp;
            }

            //----- Check user status
            if (developer.RespStatus == 0)
            {
                //----- Validate User Password
                var sec = new TSSecurity();
                if (sec.ValidatePassword(model.Password, developer.Data2, developer.Data3))
                {
                    var respData = await db.ClientsRepository.ClientLogin(Convert.ToInt32(developer.Data1), 0);
                    db.Reset();
                    if (respData == null)
                    {
                        resp.RespStatus = 1;
                        resp.RespMessage = "Technical Error- Login Aborted";
                        return resp;
                    }
                    var CheckSuccessData = respData.Where(x => x.ItemType == 1).FirstOrDefault();
                    if (int.Parse(CheckSuccessData.Data2) == 0)
                    {
                        var CheckUserData = respData.Where(x => x.ItemType == 2).FirstOrDefault();
                        if (int.Parse(CheckUserData.Data2) < 1)
                        {
                            resp.RespStatus = 1;
                            resp.RespMessage = "Technical Error- Loading User Details";
                            return resp;
                        }
                        else
                        {
                            resp = new Client();
                            resp.ClientName = CheckUserData.Data3.Trim();
                            resp.email = CheckUserData.Data4.Trim();
                            resp.ClientCode = Convert.ToInt32(CheckUserData.Data2.Trim());
                            resp.RespMessage = CheckSuccessData.Data3;
                            return resp;
                        }
                    }
                    else
                    {
                        resp.RespStatus = 1;
                        resp.RespMessage = CheckSuccessData.Data3;
                    }
                }
                else
                {
                    var respData = await db.SecurityRepository.UserLoginAsync(Convert.ToInt32(developer.Data1), 1);
                    db.Reset();

                    resp.RespStatus = 1;
                    resp.RespMessage = "Incorrect Username and/or Password!";
                }
            }
            else
            {
                resp.RespStatus = 1;
                resp.RespMessage = developer.RespMessage;
            }
            return resp;
        }
        public async Task<GenericModel> CreateClient(Client model)
        {
            TSSecurity security = new TSSecurity();
            string salt = security.GenerateSalt(25);
            string rawPass = model.RawPass;
            string password = security.HashPassword(rawPass, salt);

            model.Salt = salt;
            model.Pwd = password;
            var result = await db.ClientsRepository.RegisterClient(model);
            return result;
        }
        public async Task<IEnumerable<screenshotdets>> GetScreenshotClientAsync(int filter,string value,int clientcode)
        {
            return await db.ClientsRepository.GetScreenShotsPerClient(filter,value,clientcode);
        }
        #region projects
        public async Task<BaseEntity> CreateProject(Project project)
        {
            return await db.ClientsRepository.CreateProject(project);
        }
        public async Task<IEnumerable<Project>> GetProjectsAsync(int clientcode, int status)
        {
            return await db.ClientsRepository.Projects(clientcode,status);
        }
        public async Task<BaseEntity> InviteDeveloper(ProjectInviteModel project)
        {
            return await db.ClientsRepository.InviteDeveloper(project);
        }
        public async Task<Project> GetProjectByCode(int code)
        {
            return await db.ClientsRepository.GetProject(code);
        }
        public async Task<IEnumerable<Developer>> DevelopersPerProject(int projectcode)
        {
            return await db.ClientsRepository.DevelopersPerProject(projectcode);
        }
        #endregion
        #endregion
        #region Other Functions
        private void SendMail(string host, int port, bool ssl, string user, string pass, string subject, string toEmail, string mailMessage, List<MailAttachment> attachments = null)
        {
            Task.Run(() =>
            {
                try
                {
                    string[] userData = user.Split(',');
                    string userName = userData[0];
                    string userTitle = "";
                    if (userData.Length == 2)
                        userTitle = userData[1];

                    MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress(userName, userTitle),
                        IsBodyHtml = true
                    };

                    string[] emails = toEmail.Split(',');
                    foreach (string email in emails)
                    {
                        mail.To.Add(new MailAddress(email.Trim()));
                    }

                    mail.Subject = subject;
                    mail.Priority = MailPriority.High;
                    mail.Body = mailMessage;

                    //================== Attachments ===================
                    if (attachments != null)
                    {
                        foreach (var att in attachments)
                        {
                            if (att.ItemStream != null)
                                mail.Attachments.Add(new Attachment(att.ItemStream, att.ItemName, att.MediaType));
                        }
                    }
                    //===================================================

                    using (SmtpClient smtp = new SmtpClient(host, port))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(userName, pass);
                        smtp.EnableSsl = ssl;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //smtp.Timeout = 30000;
                        smtp.Send(mail);
                    }

                    LogUtil.Infor(LogFile, "Bl.SendMail()", "Email sent.....");
                }
                catch (Exception ex)
                {
                    LogUtil.Error(LogFile, "Bl.SendMail()", ex);
                }
            });
        }
        #endregion
    }
}