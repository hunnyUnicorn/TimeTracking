using DBL;
using DBL.Entities;
using DBL.Enums;
using DBL.Models;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Rotativa.AspNetCore;
using Stripe;
using Stripe.Checkout;
using System.Data;
using System.Reflection;
using TimeTrackerCustomers.Helpers;
using TimeTrackerCustomers.Utils;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace TimeTrackerCustomers.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DeveloperController : BaseController
    {
        private Bl bl;
        private string logFile;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DeveloperController(IOptions<AppConfig> appSett, IHostingEnvironment hostingEnvironment)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            logFile = appSett.Value.LogFile;
            bl.LogFile = logFile;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> DashBoard()
        {
            ViewBag.ScreenCasts = (await bl.GetDeveloperScreenShots(0,"0",SessionDeveloperData.DevCode)).ToList();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Projects()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProjectList(int status = 0)
        {
            var projectList = new List<Project>();
            projectList = (await bl.GetDeveloperProjects(SessionDeveloperData.DevCode)).ToList();
            return PartialView("_ProjectList", projectList);
        }
        [HttpGet]
        public async Task<IActionResult>Reports()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MyActivities()
        {
            var model = new List<TimeTrack>();
            try
            {
                model = (await bl.GetTimeTracksAsync(SessionDeveloperData.DevCode)).ToList();
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Developer.MyActivities", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> WebTracker()
        {
            var model = await bl.GetDeveloperProjects(SessionDeveloperData.DevCode);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTimeFrame(TimeTrack model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                model.DevCode = SessionDeveloperData.DevCode;
                var result = await bl.CreateTimeFrame(model);
                reqResult.Message = result.RespMessage;
                if (result.RespStatus == 0)
                {
                    reqResult.Success = true;
                    reqResult.Data = result.Data1;
                }
                else
                {
                    if (result.RespStatus == 1)
                    {
                        Danger(result.RespMessage);
                    }
                    else
                    {
                        LogUtil.Error(logFile, "Developer.CreateTimeFrame", new Exception(result.RespMessage));
                        Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.CreateTimeFrame", ex);
            }
            return Json(reqResult);
        }
        [HttpPost]
        public async Task<IActionResult> StopTimeFrame(int TTCode, int KeyHits, int mouseHits)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var result = await bl.StopTimeFrame(TTCode,KeyHits,mouseHits);
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
                        LogUtil.Error(logFile, "Developer.StopTimeFrame", new Exception(result.RespMessage));
                        Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.StopTimeFrame", ex);
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> ProjectInvites()
        {
            var model = new List<ProjectInvite>();
            try
            {
                model = (await bl.GetProjectInvitesAsync(SessionDeveloperData.DevCode)).ToList();
            }
            catch (Exception ex)
            {
                Danger("Error occured when getting the project invite");
                LogUtil.Error(logFile, "Developer.StopTimeFrame", ex);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ProjectInviteAction(int ProjectCode)
        {
            var model = new ProjectInvite();
            try
            {
                model = await bl.GetProjectInviteAsync(ProjectCode);
                if(model.RespStatus == 1)
                {
                    Danger(model.RespMessage);
                    return RedirectToAction("ProjectInvites");
                }
            }
            catch(Exception ex)
            {
                Danger("Error occured when getting the project invite");
                LogUtil.Error(logFile, "Developer.StopTimeFrame", ex);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> InviteTakeAction(int action, int invitecode, string reason)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var result = await bl.Invite_Action(action, SessionDeveloperData.DevCode, invitecode, reason);
                if (result.RespStatus == 0)
                {
                    Success(result.RespMessage);
                    reqResult.Success = true;
                }
                else if (result.RespStatus == -1) //failed posting
                {
                    Danger(result.RespMessage);
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
                        LogUtil.Error(logFile, "Developer.InviteTakeAction", new Exception(result.RespMessage));
                        Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.InviteTakeAction", ex);
                Danger("Action failed due to an error!");
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> ScreenCasts()
        {
            var model = new List<screenshotdets>();
            try
            {
                await LoadScreenCastFilterItems(SessionDeveloperData.DevCode);
                model = (await bl.GetDeveloperScreenShots(0, "0", SessionDeveloperData.DevCode)).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Client.GetProject()", ex);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ScreenCastList(int filter, string value)
        {
            var model = new List<screenshotdets>();
            try
            {
                model = (await bl.GetDeveloperScreenShots(filter, value, SessionDeveloperData.DevCode)).ToList();
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.ScreenCastList()", ex);
            }
            return PartialView("_ScreenCastList", model);
        }
        [HttpGet]
        public async Task<IActionResult> ProfileSettings()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Invoices()
        {
            var model = new List<TimeTrackerInvoice>();
            try
            {
                model = (await bl.GetDeveloperInvoices(SessionDeveloperData.DevCode)).ToList();
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
        public async Task<IActionResult> InvoicePreview(int invoicecode)
        {
            var invoicedets = new List<InvoiceDets>();
            invoicedets = (await bl.GetInvoiceDets(invoicecode)).ToList();

            var devName = invoicedets.Where(x => x.Item == "Dev_Name" && x.ItemType == 3).FirstOrDefault().ItemValue;
            var invoiceref = invoicedets.Where(x => x.Item == "Invoice_Ref" && x.ItemType == 0).FirstOrDefault().ItemValue;
            var dateGen = invoicedets.Where(x => x.Item == "Gen_Date" && x.ItemType == 0).FirstOrDefault().ItemValue;
            var ClientName = invoicedets.Where(x => x.Item == "Client_Name" && x.ItemType == 2).FirstOrDefault().ItemValue;
            var projectName = invoicedets.Where(x => x.Item == "Project_Title" && x.ItemType == 1).FirstOrDefault().ItemValue;

            List<InvoiceData> invoiceData = new List<InvoiceData>();
            foreach(var indata in invoicedets.Where(x=>x.ItemType==4))
            {
                invoiceData.Add(new InvoiceData { 
                    Item = indata.Item,
                    ItemDescr = indata.ItemDescr,
                     ItemType = indata.ItemType,
                     ItemValue = Convert.ToDecimal(indata.ItemValue)
                });
            }
            FastReport.Report webReport = new FastReport.Report();
            var newFilePdf = invoiceref + ".pdf";
            webReport.Report.Load(Path.Combine(Environment.CurrentDirectory,"Reports","invoice.frx"));
            webReport.Report.Dictionary.Connections.Clear();
            var dts = SessionHelper.ToDataSet<InvoiceData>(invoiceData);
            webReport.Report.RegisterData(dts.Tables[0], "Data");
            webReport.GetDataSource("Data").Enabled = true;
            webReport.SetParameterValue("DeveloperName", devName);
            webReport.SetParameterValue("InvoiceNo", invoiceref);
            webReport.SetParameterValue("DateGen", dateGen);
            webReport.SetParameterValue("ClientName", ClientName);
            webReport.SetParameterValue("ProjectName", projectName);
            (webReport.FindObject("Data1") as DataBand).DataSource = webReport.GetDataSource("Data");


            webReport.Report.Prepare();
            // save file in stream
            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, newFilePdf);
            }
        }
        [HttpPost]
        public async Task<IActionResult>InvoiceEmail(int invoicecode)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                var invoicedets = new List<InvoiceDets>();
                invoicedets = (await bl.GetInvoiceDets(invoicecode)).ToList();

                var devName = invoicedets.Where(x => x.Item == "Dev_Name" && x.ItemType == 3).FirstOrDefault().ItemValue;
                var invoiceref = invoicedets.Where(x => x.Item == "Invoice_Ref" && x.ItemType == 0).FirstOrDefault().ItemValue;
                var dateGen = invoicedets.Where(x => x.Item == "Gen_Date" && x.ItemType == 0).FirstOrDefault().ItemValue;
                var ClientName = invoicedets.Where(x => x.Item == "Client_Name" && x.ItemType == 2).FirstOrDefault().ItemValue;
                var projectName = invoicedets.Where(x => x.Item == "Project_Title" && x.ItemType == 1).FirstOrDefault().ItemValue;

                List<InvoiceData> invoiceData = new List<InvoiceData>();
                foreach (var indata in invoicedets.Where(x => x.ItemType == 4))
                {
                    invoiceData.Add(new InvoiceData
                    {
                        Item = indata.Item,
                        ItemDescr = indata.ItemDescr,
                        ItemType = indata.ItemType,
                        ItemValue = Convert.ToDecimal(indata.ItemValue)
                    });
                }
                FastReport.Report webReport = new FastReport.Report();
                var newFilePdf = invoiceref + ".pdf";
                webReport.Report.Load(Path.Combine(Environment.CurrentDirectory, "Reports", "invoice.frx"));
                webReport.Report.Dictionary.Connections.Clear();
                var dts = SessionHelper.ToDataSet<InvoiceData>(invoiceData);
                webReport.Report.RegisterData(dts.Tables[0], "Data");
                webReport.GetDataSource("Data").Enabled = true;
                webReport.SetParameterValue("DeveloperName", devName);
                webReport.SetParameterValue("InvoiceNo", invoiceref);
                webReport.SetParameterValue("DateGen", dateGen);
                webReport.SetParameterValue("ClientName", ClientName);
                webReport.SetParameterValue("ProjectName", projectName);
                (webReport.FindObject("Data1") as DataBand).DataSource = webReport.GetDataSource("Data");


                webReport.Report.Prepare();
                // save file in stream
                using (MemoryStream ms = new MemoryStream())
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    pdfExport.Export(webReport.Report, ms);
                    ms.Flush();
                    await bl.SendClientInvoice(ms.ToArray(), invoicecode, invoiceref);
                   
                }
                Success("Email sent successfully");
            }
            catch(Exception ex)
            {
                LogUtil.Error(logFile, "Developer.CreateInvoice", ex);
                Danger("Action failed due to a database error!");
            }
           
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> Invoice()
        {
            var model = new InvoiceData();
            await LoadInvoiceItems(SessionDeveloperData.DevCode);
            return PartialView("_invoice",model);
        }
        [HttpPost]
        public async Task<IActionResult> Invoice(TimeTrackerInvoice model)
        {
            ReqResult reqResult = new ReqResult { Success = false };
            try
            {
                model.DeveloperCode = SessionDeveloperData.DevCode;
                var result = await bl.CreateInvoice(model);
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
                        LogUtil.Error(logFile, "Developer.CreateInvoice", new Exception(result.RespMessage));
                        // Danger("Action failed due to a database error!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logFile, "Developer.CreateInvoice", ex);
                reqResult.Message = "Action failed due to a database error!";
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> SubscriptionManagement()
        {
            var model = new SubPlanDets();
            model = await bl.GetDeveloperPlanDetails(SessionDeveloperData.DevCode);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> RenewSubscription()
        {
            await SubscriptionItems(1);
            return PartialView("_RenewSubscription");
        }
        [HttpPost]
        public async Task<IActionResult> RenewSubscription(RenewSub model)
        {
            ReqResult reqResult= new ReqResult { Success = true };
            try
            {
                var stripeDets = await bl.CreateTxn(model.SubPlanCode, SessionDeveloperData.DevCode);
                string baseUrl = string.Format("{0}://{1}",
                      HttpContext.Request.Scheme, HttpContext.Request.Host);
                var options = new SessionCreateOptions
                {
                    SuccessUrl = String.Format("{0}/Developer/PaymentSuccess", baseUrl),
                    CancelUrl = String.Format("{0}/Developer/PaymentCancel", baseUrl),
                    ClientReferenceId = stripeDets.ClientReferenceId,
                    Currency = stripeDets.currency.ToLower(),
                    CustomerEmail = stripeDets.Email,
                    PaymentIntentData = new SessionPaymentIntentDataOptions { 
                        Shipping = new ChargeShippingOptions { 
                            Address = new AddressOptions
                            {
                                City = "",
                                Country="",
                                Line1 = "",
                                Line2 = "",
                                PostalCode = "",
                                State = ""
                            }
                        },
                    },
                    LineItems = new List<SessionLineItemOptions>
            {
            new SessionLineItemOptions
            {
               PriceData = new
               SessionLineItemPriceDataOptions{
                Currency =stripeDets.currency,
                UnitAmountDecimal = stripeDets.Amount,
                ProductData = new SessionLineItemPriceDataProductDataOptions{
                    Name = stripeDets.ProductName,
                }
               },
               Quantity = 1,
               
               
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
            catch(Exception ex)
            {
                reqResult.Success = false;
            }
            return Json(reqResult);
        }
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess()
        {
           var sessionid =  SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "stripeSessionId");
           var txncode =  SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "txnCode");
           var service = new SessionService();
           var session = service.Get(sessionid);
            await bl.UpdateStripeDets(txncode, sessionid,session.PaymentIntentId );
            return RedirectToAction("SubscriptionManagement");
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCancel()
        {
            return View();
        }
        private async Task LoadScreenCastFilterItems(int role = 0)
        {
            var list = (await bl.GetItemListAsync(ListItemType.DeveloperProjects, role)).Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            ViewData["Projects"] = list;
        }
        private async Task LoadInvoiceItems(int role = 0)
        {
            var list = (await bl.GetItemListAsync(ListItemType.DeveloperProjects, role)).Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();

            ViewData["Projects"] = list;
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
 