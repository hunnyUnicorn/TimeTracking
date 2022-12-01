using DBL.Models;
using DBL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DBL.Entities;
using DBL.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Newtonsoft.Json.Linq;
using Stripe.Checkout;
using TimeTrackerCustomers.Helpers;

namespace TimeTrackerCustomers.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ClientController : BaseController
    {
        private Bl bl;
        private string logFile;
        public ClientController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
        }
        [HttpGet]
        public async Task<IActionResult> DashBoard()
        {
            ViewBag.ScreenCasts = (await bl.GetScreenshotClientAsync(0,"0",SessionClientData.ClientCode)).ToList();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>Projects()
        {
            await LoadProjectItems();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProjectList(int status=0)
        {
            var projectList = new List<Project>();
            projectList = (await bl.GetProjectsAsync(SessionClientData.ClientCode, status)).ToList();
            return PartialView("_ProjectList", projectList);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(Project model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                model.ClientCode = SessionClientData.ClientCode;
                model.ProjRef = Guid.NewGuid().ToString();
                var result = await bl.CreateProject(model);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                    reqResult.Success = true;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                        // Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Client.CreateProject", new Exception(result.RespMessage));
                        // Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Client.CreateProject()", ex);
            }
            return Json(reqResult);
        }
        [HttpPost]
        public async Task<IActionResult> InviteDeveloper(ProjectInviteModel model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                model.ClientCode = SessionClientData.ClientCode;
                var result = await bl.InviteDeveloper(model);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                    reqResult.Success = true;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                        Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Client.InviteDeveloper", new Exception(result.RespMessage));
                        Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Client.InviteDeveloper()", ex);
                Danger("Action failed due to a database error!");
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> ProjectManage(int projectCode)
        {
            Project project = new Project();
            try
            {
                project = await bl.GetProjectByCode(projectCode);
                ViewBag.ScreenCasts = (await bl.GetScreenshotClientAsync(1, projectCode.ToString(), SessionClientData.ClientCode)).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Client.GetProject()", ex);
            }
            return View(project);
        }
        [HttpGet]
        public async Task<IActionResult> ProjectDevelopers(int projectcode)
        {
            var developersList = new List<Developer>();
            developersList = (await bl.DevelopersPerProject(projectcode)).ToList();
            return PartialView("_ProjectDevelopers", developersList);
        }
        [HttpGet]
        public async Task<IActionResult> ScreenCasts()
        {
            var model = new List<screenshotdets>();
            try
            {
                await LoadScreenCastFilterItems(SessionClientData.ClientCode);
                model = (await bl.GetScreenshotClientAsync(0,"0",SessionClientData.ClientCode)).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Client.GetProject()", ex);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ScreenCastList(int filter,string value)
        {
            var model = new List<screenshotdets>();
            try
            {
                model = (await bl.GetScreenshotClientAsync(filter,value,SessionClientData.ClientCode)).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Client.GetProject()", ex);
            }
            return PartialView("_ScreenCastList", model);
        }
        [HttpGet]
        public async Task<IActionResult> ClientProfile()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ActivityLog()
        {
            var model = new List<TimeTrack>();
            try
            {
                model = (await bl.GetTimeTracksClientAsync(SessionClientData.ClientCode)).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Client.ActivityLog()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Invoices()
        {
            var model = new List<TimeTrackerInvoice>();
            try
            {
                model = (await bl.GetClientInvoicesAsync(SessionClientData.ClientCode)).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.Invoices()", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> InvoiceView(int invoicecode)
        {
            var invoicedets = new List<InvoiceDets>();
            invoicedets = (await bl.GetInvoiceDets(invoicecode)).ToList();
            ViewBag.DevName = invoicedets.Where(x => x.Item == "Dev_Name" && x.ItemType == 3).FirstOrDefault().ItemValue;
            ViewBag.Invoiceref = invoicedets.Where(x => x.Item == "Invoice_Ref" && x.ItemType == 0).FirstOrDefault().ItemValue;
            ViewBag.DateGen = invoicedets.Where(x => x.Item == "Gen_Date" && x.ItemType == 0).FirstOrDefault().ItemValue;
            ViewBag.ClientName = invoicedets.Where(x => x.Item == "Client_Name" && x.ItemType == 2).FirstOrDefault().ItemValue;
            ViewBag.ProjectName = invoicedets.Where(x => x.Item == "Project_Title" && x.ItemType == 1).FirstOrDefault().ItemValue;
            ViewBag.InvoiceCode = invoicecode;
            return View("InvoiceView", invoicedets);
        }
        [HttpGet]
        public async Task<IActionResult> SubscriptionManagement()
        {
            var model = new SubPlanDets();
            model = await bl.GetClientPlanDetails(SessionClientData.ClientCode);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> RenewSubscription()
        {
            await SubscriptionItems(2);
            return PartialView("_RenewSubscription");
        }
        [HttpPost]
        public async Task<IActionResult> RenewSubscription(RenewSub model)
        {
            ReqResult reqResult = new ReqResult { Success = true };
            try
            {
                var stripeDets = await bl.CreateTxn(model.SubPlanCode, SessionClientData.ClientCode);
                string baseUrl = string.Format("{0}://{1}",
                      HttpContext.Request.Scheme, HttpContext.Request.Host);
                var options = new SessionCreateOptions
                {
                    SuccessUrl = String.Format("{0}/Client/PaymentSuccess", baseUrl),
                    CancelUrl = String.Format("{0}/Client/PaymentCancel", baseUrl),
                    ClientReferenceId = stripeDets.ClientReferenceId,
                    Currency = stripeDets.currency.ToLower(),
                    CustomerEmail = stripeDets.Email,
                    LineItems = new List<SessionLineItemOptions>
            {
            new SessionLineItemOptions
            {
               PriceData = new SessionLineItemPriceDataOptions{
                Currency =stripeDets.currency,
                UnitAmountDecimal = stripeDets.Amount,
                ProductData = new SessionLineItemPriceDataProductDataOptions{
                    Name = stripeDets.ProductName,
                }
               },
               Quantity = 1
            },
            },
                    Mode = stripeDets.mode,
                };
                var service = new SessionService();
                Session session = service.Create(options);
                reqResult.Data = session.Url;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "stripeSessionId", session.Id);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "txnCode", stripeDets.ClientReferenceId);
            }
            catch (Exception ex)
            {
                reqResult.Success = false;
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess()
        {
            var sessionid = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "stripeSessionId");
            var txncode = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "txnCode");
            var service = new SessionService();
            var session = service.Get(sessionid);
            await bl.UpdateStripeDets(txncode, sessionid, session.PaymentIntentId);
            return RedirectToAction("SubscriptionManagement");
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCancel()
        {
            return View();
        }
        private async Task LoadScreenCastFilterItems(int role = 0)
        {
            var list = (await bl.GetItemListAsync(ListItemType.ProjectDevelopers, role)).Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            ViewData["Developers"] = list;
        }
        private async Task LoadProjectItems(int role = 0)
        {
            var list = (await bl.GetItemListAsync(ListItemType.ProjectCategories, role)).Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            ViewData["categories"] = list;
            list = (await bl.GetItemListAsync(ListItemType.Currencies, role)).Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            ViewData["Currencies"] = list;
        }
        private async Task SubscriptionItems(int role = 0)
        {
            var list = (await bl.GetItemListAsync(ListItemType.Subscriptions, role)).Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            ViewData["Subcriptions"] = list;
        }
    }
}
