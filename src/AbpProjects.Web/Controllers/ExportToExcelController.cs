using Abp.UI;
using AbpProjects.MeghPlanningSupportServices;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.Reports;
using AbpProjects.Reports.Dto;
using AbpProjects.TimeSheet;
using AbpProjects.TimeSheet.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hangfire;
using Abp.Net.Mail;
using AbpProjects.BulkData;
using AbpProjects.BulkData.Dto;
using AbpProjects.Utilities;
using ExcelDataReader;
using System.Threading.Tasks;
using AbpProjects.Project;
using AbpProjects.Project.Dto;
using Abp.Application.Services.Dto;
using AbpProjects.ImportUserStoryData;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.Opportunities;
using AbpProjects.OpportunityAppServices;
using Abp.Domain.Repositories;
using AbpProjects.LeaveApplication;

namespace AbpProjects.Web.Controllers
{
    public class ExportToExcelController : Controller
    {

        private readonly IBulkDataAppService _bulkDataRepository;
        private readonly IOpportunityService _bulkOpportunityDataRepository;
        private readonly IReportsAppService _reprotsAppService;
        private readonly ITimeSheetAppService _timeSheetAppService;
        private readonly ISupportAppService _supportAppService;
        private readonly IExcelReaderUtility _readerUtility;
        private readonly IProjectAppService _projectAppService;
        private readonly IImportUserStoryDetailsAppService _userStoryService;
        private readonly ILeaveApplicationAppService _leaveapplicationRepository;
        private readonly IOpportunityService _iOpportunityService;

        public ExportToExcelController(IBulkDataAppService bulkDataRepository, IReportsAppService reprotsAppService, ITimeSheetAppService timeSheetAppService,
            ISupportAppService supportAppService, IExcelReaderUtility readerUtility,
            IProjectAppService projectAppService, IImportUserStoryDetailsAppService userStoryService,
            IOpportunityService bulkOpportunityDataRepository, ILeaveApplicationAppService leaveapplicationRepository, IOpportunityService iOpportunityService
            )
        {
            _bulkOpportunityDataRepository = bulkOpportunityDataRepository;
            _bulkDataRepository = bulkDataRepository;
            _reprotsAppService = reprotsAppService;
            _timeSheetAppService = timeSheetAppService;
            _supportAppService = supportAppService;
            _readerUtility = readerUtility;
            _projectAppService = projectAppService;
            _userStoryService = userStoryService;
            _leaveapplicationRepository = leaveapplicationRepository;
            _iOpportunityService = iOpportunityService;
        }

        //public async Task<ActionResult> SaveOpportunityData(HttpPostedFileBase file)
        //{
        //    ImportExcelData.Dto.ExcelValidationList excel = new ImportExcelData.Dto.ExcelValidationList();
        //    excel.isnullcolumn = false;
        //    string rows = "row ";
        //    var rowcount = new List<string>();
        //    //var nullcolumnscount = new List<string>();
        //    //var notexsistcolumnscount = new List<string>();
        //    var errorcount = new List<string>();

        //    if (file != null)
        //    {
        //        var file_Name = "BulkData_" + DateTime.Now.Ticks.ToString() + ".xlsx";
        //        string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "Temp\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
        //        file.SaveAs(fileName);
        //        List<string> extension = new List<string> { ".xls", "xlsx" };
        //        //    var fileName = await _bulkDataRepository.SaveDocument("Temp", file, extension);
        //        DataTable dt = new DataTable();
        //        // DataSet ds = _bulkDataRepository.GetDataFromExcel(fileName);
        //        DataSet ds = GetDataFromExcel(fileName);

        //        dt = ds.Tables[0];
        //        int i = 0;
        //        DataTable Customdt = new DataTable();
        //        Customdt.Columns.Add("CompanyName", typeof(System.String));
        //        Customdt.Columns.Add("PersonName", typeof(System.String));
        //        Customdt.Columns.Add("EmailId", typeof(System.String));
        //        Customdt.Columns.Add("MobileNo", typeof(System.String));
        //        Customdt.Columns.Add("CallCategory", typeof(System.String));




        //        List<Opportunity> Bulkitemdata = new List<Opportunity>();
        //        try
        //        {

        //            Bulkitemdata = (from DataRow row in dt.Rows

        //                            select new Opportunity
        //                            {
        //                                CompanyName = Convert.ToString(row["CompanyName"]),
        //                                PersonName = Convert.ToString(row["PersonName"]),
        //                                EmailId = Convert.ToString(row["EmailId"]),
        //                                MobileNumber = Convert.ToString(row["MobileNo"]),
        //                                CalllCategoryId = Convert.ToInt32(row["CallCategory"])
        //                            }).ToList();

        //            InsertOpportunityBulkData insertitem = new InsertOpportunityBulkData();
        //            insertitem.BulkOpportunityItemsData = Bulkitemdata;
        //            await _bulkOpportunityDataRepository.SaveBulkDataInDB(insertitem);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult ExportReportToExcel(GetInputDto input)
        {
            DataTable dt = _reprotsAppService.GetAllData(input);
            string FileName = "User TimeSheet Report" + DateTime.Now.Ticks + ".xls";
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);


            gv.RenderControl(htw);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
        public ActionResult ExportGetProjectsWithoutClientList(ImportGetProjectDto input)
        {
            var ProjectList = _projectAppService.ExportGetProjectsWithoutClientList(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["ProjectName"] = item.ProjectName;
                excelFields["Marketing Lead"] = item.MarketingLeadName.ToString();
                excelFields["Start Date"] = item.StartDate.ToString();
                excelFields["End Date"] = item.EndDate.ToString();
                excelFields["Priority"] = item.Priority.ToString();
                excelFields["Price"] = item.Price.ToString();
                excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                excelFields["Pending Amount"] = item.PendingAmount.ToString();
                excelFields["Actual Hours"] = item.actualhours.ToString();
                excelFields["Status"] = item.ProjectStatus.ToString();
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "ProjectsWithoutClientList" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
        public ActionResult ExportGetOutstandingInvoiceList(ImportOutStandingInvoiceDto input)
        {
            var OutstandingInvoiceList = _reprotsAppService.ExportToExcelGetOutStandingClient(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in OutstandingInvoiceList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["Invoice No"] = item.InvoiceNo;
                excelFields["Invoice Date"] = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.BillDate));
                excelFields["Client Name"] = item.ClientName.ToString();
                excelFields["Total Amount"] = item.TotalBillAmt.ToString();
                excelFields["Total Collection"] = item.TotalCollection.ToString();
                excelFields["OutStanding Amount"] = item.OutStandingAmt.ToString();
                //excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                //excelFields["Pending Amount"] = item.PendingAmount.ToString();
                //excelFields["Actual Hours"] = item.actualhours.ToString();
                //excelFields["Status"] = item.ProjectStatus.ToString();
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "OutstandingInvoiceList" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
        public ActionResult ExportOnHoldProjectToExcel(ImportGetProjectDto input)
        {
            var ProjectList = _projectAppService.ExportGetHoldProjectList(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["ProjectName"] = item.ProjectName;
                excelFields["Marketing Lead"] = item.MarketingLeadName.ToString();
                excelFields["Start Date"] = item.StartDate.ToString();
                excelFields["End Date"] = item.EndDate.ToString();
                excelFields["Priority"] = item.Priority.ToString();
                excelFields["Price"] = item.Price.ToString();
                excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                excelFields["Pending Amount"] = item.PendingAmount.ToString();
                excelFields["Actual Hours"] = item.actualhours.ToString();
                excelFields["Status"] = item.ProjectStatus.ToString();
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "OnHoldProjectList" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
        public ActionResult ExportCompleteProjectToExcel(ImportGetProjectDto input)
        {
            var ProjectList = _projectAppService.ExportGetCompletedProjectList(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["ProjectName"] = item.ProjectName;
                excelFields["Marketing Lead"] = item.MarketingLeadName.ToString();
                excelFields["Start Date"] = item.StartDate.ToString();
                excelFields["End Date"] = item.EndDate.ToString();
                excelFields["Priority"] = item.Priority.ToString();
                excelFields["Price"] = item.Price.ToString();
                excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                excelFields["Pending Amount"] = item.PendingAmount.ToString();
                excelFields["Actual Hours"] = item.actualhours.ToString();
                excelFields["Status"] = item.ProjectStatus.ToString();
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "CompletedProjectList" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }

        //OpportunityReport
        public ActionResult ExportOpportunityReportToExcel(GetImportUserstoryDto input)
        {
            var ProjectList = _userStoryService.ExportUserStory(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["User Story"] = item.UserStory;
                excelFields["Project Name"] = item.ProjectName;
                excelFields["Creation Date"] = item.CreationDate.ToString();
                excelFields["Developers Hours"] = item.DeveloperHours.ToString();
                excelFields["Expected Hours"] = item.ExpectedHours.ToString();
                excelFields["Actual Hours"] = item.ActualHours.ToString();

                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "UserStory" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
        public ActionResult ExportUserStoryReportToExcel(GetImportUserstoryDto input)
        {
            var ProjectList = _userStoryService.ExportUserStory(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["User Story"] = item.UserStory;
                excelFields["Project Name"] = item.ProjectName;
                excelFields["Creation Date"] = item.CreationDate.ToString();
                excelFields["Developers Hours"] = item.DeveloperHours.ToString();
                excelFields["Expected Hours"] = item.ExpectedHours.ToString();
                excelFields["Actual Hours"] = item.ActualHours.ToString();
                excelFields["Status"] = item.status.ToString() == "0" ? "Pending" : "Completed";
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "UserStory" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }

        public ActionResult ExportProjectStateAmountToExcel(GetInputDto input)
        {
            var ProjectList = _reprotsAppService.ExportProjectStateAmount(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["ProjectName"] = item.ProjectName;
                excelFields["ProjectCost"] = item.ProjectCost.ToString();
                excelFields["CompanyCost"] = item.CompanyCost.ToString();
                excelFields["%"] = item.CostPercentage.ToString();
                excelFields["Profit"] = item.Profit.ToString();
                excelFields["Profit (%)"] = item.ProfitPercentage.ToString();
                excelFields["Status"] = item.Status;

                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "ProjectStateAmount" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }

        static DataTable ToDataTable(List<Dictionary<string, string>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();
            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, string> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }
            return result;
        }
        public JsonResult GetServiceDomainNames(string q)
        {
            var result = new List<DomainListDto>();
            try
            {
                result = _supportAppService.GetDomainNameList(q);
            }
            catch (Exception ex)
            {

            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveBulkData(HttpPostedFileBase file)
        {
            ExcelValidationList excel = new ExcelValidationList();
            excel.isnullcolumn = false;
            string rows = "row ";
            var rowcount = new List<string>();
            //var nullcolumnscount = new List<string>();
            //var notexsistcolumnscount = new List<string>();
            var errorcount = new List<string>();

            if (file != null)
            {
                var file_Name = "BulkData_" + DateTime.Now.Ticks.ToString() + ".xls";
                string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "Temp\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                file.SaveAs(fileName);
                List<string> extension = new List<string> { ".xls" };
                //    var fileName = await _bulkDataRepository.SaveDocument("Temp", file, extension);
                DataTable dt = new DataTable();
                DataSet ds = _bulkDataRepository.GetDataFromExcel(fileName);

                dt = ds.Tables[0];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    if (i % 4 == 1)
                    {
                        Console.Write(dr[0].ToString());
                    }
                    bool insert = false;
                    if (i % 4 == 3)
                    {
                        for (int j = 0; j < 31; j++)
                        {
                            Console.Write("day" + (j + 1).ToString() + "==>" + dr[j].ToString());
                        }
                        insert = true;
                    }

                }
                List<bulkmaster> Bulkitemdata = new List<bulkmaster>();
                try
                {

                    Bulkitemdata = (from DataRow row in dt.Rows

                                    select new bulkmaster
                                    {
                                        EmpId = Convert.ToInt32(row["EmpId"]),
                                        EmpName = Convert.ToString(row["EmpName"]),
                                        EmpEmail = Convert.ToString(row["EmpEmail"]),
                                        Contact = Convert.ToInt32(row["Contact"]),
                                    }).ToList();

                    InsertBulkData insertitem = new InsertBulkData();
                    insertitem.BulkItemsData = Bulkitemdata;
                    await _bulkDataRepository.SaveBulkDataInDB(insertitem);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> SaveLoginPunchData(HttpPostedFileBase file)
        {

            if (file != null)
            {
                var file_Name = "BulkData_" + DateTime.Now.Ticks.ToString() + ".xls";
                string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "Temp\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                file.SaveAs(fileName);
                List<string> extension = new List<string> { ".xls" };
                //    var fileName = await _bulkDataRepository.SaveDocument("Temp", file, extension);
                DataTable dt = new DataTable();
                DataSet ds = _bulkDataRepository.GetDataFromExcel(fileName);

                dt = ds.Tables[0];
                int i = 0;
                DataTable Customdt = new DataTable();
                Customdt.Columns.Add("Employeecode", typeof(System.String));
                Customdt.Columns.Add("INTime", typeof(System.String));
                Customdt.Columns.Add("OutTime", typeof(System.String));
                Customdt.Columns.Add("OutTime", typeof(System.String));



                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    if (i % 4 == 1)
                    {
                        Console.Write(dr[0].ToString());
                    }
                    bool insert = false;
                    if (i % 4 == 3)
                    {
                        for (int j = 0; j < 31; j++)
                        {
                            Console.Write("day" + (j + 1).ToString() + "==>" + dr[j].ToString());
                        }
                        insert = true;
                    }

                }
                List<bulkmaster> Bulkitemdata = new List<bulkmaster>();
                try
                {

                    Bulkitemdata = (from DataRow row in dt.Rows

                                    select new bulkmaster
                                    {
                                        EmpId = Convert.ToInt32(row["EmpId"]),
                                        EmpName = Convert.ToString(row["EmpName"]),
                                        EmpEmail = Convert.ToString(row["EmpEmail"]),
                                        Contact = Convert.ToInt32(row["Contact"]),
                                    }).ToList();

                    InsertBulkData insertitem = new InsertBulkData();
                    insertitem.BulkItemsData = Bulkitemdata;
                    await _bulkDataRepository.SaveBulkDataInDB(insertitem);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadDocument(HttpPostedFileBase Files)
        {
            var file = Request.Files[0];
            if (file != null)
            {
                List<string> extension = new List<string> { ".pdf", ".jpg", ".jpeg", ".doc", ".docx", ".txt", ".xls", ".xlsx" };
                var fileName = _readerUtility.SaveAttachment("Download", file, extension);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportReportToExcelProjectAll(ImportGetProjectDto input)
        {
            try
            {
                var ProjectList = _projectAppService.GetAllProjectListData(input);
                var excelData = new List<Dictionary<string, string>>();
                foreach (var item in ProjectList)
                {
                    Dictionary<string, string> excelFields = new Dictionary<string, string>();
                    excelFields["ProjectName"] = item.ProjectName;
                    excelFields["Marketing Lead"] = item.MarketingLeadName;
                    excelFields["StartDate"] = item.StartDate.ToString();
                    excelFields["EndDate"] = item.EndDate.ToString();
                    excelFields["Priority"] = item.Priority.ToString();
                    excelFields["Price"] = item.Price.ToString();
                    excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                    excelFields["Pending amount"] = item.PendingAmount.ToString();
                    excelFields["ActualHours"] = (item.actualhours + "/" + item.totalhours).ToString();
                    //excelFields["Type"] = item.Status;
                    excelFields["Status"] = item.ProjectStatus;

                    excelData.Add(excelFields);
                }
                DataTable dt = ToDataTable(excelData);

                string FileName = "AllProjects" + DateTime.Now.Ticks + ".xls";

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);

                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
                return Json(new { fileName = FileName });
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public ActionResult ExportReportToExcelProjectActive(ImportGetProjectDto input)
        {
            try
            {
                var ProjectList = _projectAppService.GetActiveProjectListData(input);
                var excelData = new List<Dictionary<string, string>>();
                foreach (var item in ProjectList)
                {
                    Dictionary<string, string> excelFields = new Dictionary<string, string>();
                    excelFields["ProjectName"] = item.ProjectName;
                    excelFields["Marketing Lead"] = item.MarketingLeadName;
                    excelFields["StartDate"] = item.StartDate.ToString();
                    excelFields["EndDate"] = item.EndDate.ToString();
                    excelFields["Priority"] = item.Priority.ToString();
                    excelFields["Price"] = item.Price.ToString();
                    excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                    excelFields["Pending amount"] = item.PendingAmount.ToString();
                    excelFields["ActualHours"] = (item.actualhours + "/" + item.totalhours).ToString();
                    //excelFields["Type"] = item.Status;
                    excelFields["Status"] = item.ProjectStatus;

                    excelData.Add(excelFields);
                }
                DataTable dt = ToDataTable(excelData);

                string FileName = "ActiveProjects" + DateTime.Now.Ticks + ".xls";

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);

                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
                return Json(new { fileName = FileName });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public ActionResult ExportReportToExcelProjectAMC(ImportGetProjectDto input)
        {
            try
            {
                var ProjectList = _projectAppService.GetAmcProjectListAMC(input);
                var excelData = new List<Dictionary<string, string>>();
                foreach (var item in ProjectList)
                {
                    Dictionary<string, string> excelFields = new Dictionary<string, string>();
                    excelFields["ProjectName"] = item.ProjectName;
                    excelFields["Marketing Lead"] = item.MarketingLeadName;
                    excelFields["StartDate"] = item.StartDate.ToString();
                    excelFields["EndDate"] = item.EndDate.ToString();
                    excelFields["Priority"] = item.Priority.ToString();
                    excelFields["Price"] = item.Price.ToString();
                    excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                    excelFields["Pending amount"] = item.PendingAmount.ToString();
                    excelFields["ActualHours"] = (item.actualhours + "/" + item.totalhours).ToString();
                    //excelFields["Type"] = item.Status;
                    excelFields["Status"] = item.ProjectStatus;

                    excelData.Add(excelFields);
                }
                DataTable dt = ToDataTable(excelData);

                string FileName = "AMCProjects" + DateTime.Now.Ticks + ".xls";

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);

                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
                return Json(new { fileName = FileName });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public ActionResult ExportReportToExcelProjectONGOING(ImportGetProjectDto input)
        {
            try
            {
                var ProjectList = _projectAppService.GetOnGoingProjectListOnGoing(input);
                var excelData = new List<Dictionary<string, string>>();
                foreach (var item in ProjectList)
                {
                    Dictionary<string, string> excelFields = new Dictionary<string, string>();
                    excelFields["ProjectName"] = item.ProjectName;
                    excelFields["Marketing Lead"] = item.MarketingLeadName;
                    excelFields["StartDate"] = item.StartDate.ToString();
                    excelFields["EndDate"] = item.EndDate.ToString();
                    excelFields["Priority"] = item.Priority.ToString();
                    excelFields["Price"] = item.Price.ToString();
                    excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                    excelFields["Pending amount"] = item.PendingAmount.ToString();
                    excelFields["ActualHours"] = (item.actualhours + "/" + item.totalhours).ToString();
                    //excelFields["Type"] = item.Status;
                    excelFields["Status"] = item.ProjectStatus;

                    excelData.Add(excelFields);
                }
                DataTable dt = ToDataTable(excelData);

                string FileName = "On Going Projects" + DateTime.Now.Ticks + ".xls";

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);

                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
                return Json(new { fileName = FileName });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public ActionResult ExportReportToExcelProjectInvoice(ImportGetProjectDto input)
        {
            try
            {
                var ProjectList = _projectAppService.GetInvoiceCollectionProjectListINVOICE(input);
                var excelData = new List<Dictionary<string, string>>();
                foreach (var item in ProjectList)
                {
                    Dictionary<string, string> excelFields = new Dictionary<string, string>();
                    excelFields["ProjectName"] = item.ProjectName;
                    excelFields["Marketing Lead"] = item.MarketingLeadName;
                    excelFields["StartDate"] = item.StartDate.ToString();
                    excelFields["EndDate"] = item.EndDate.ToString();
                    excelFields["Priority"] = item.Priority.ToString();
                    excelFields["Price"] = item.Price.ToString();
                    excelFields["Invoice Amount"] = item.Invoiceamount.ToString();
                    excelFields["Pending amount"] = item.PendingAmount.ToString();
                    excelFields["ActualHours"] = (item.actualhours + "/" + item.totalhours).ToString();
                    //excelFields["Type"] = item.Status;
                    excelFields["Status"] = item.ProjectStatus;

                    excelData.Add(excelFields);
                }
                DataTable dt = ToDataTable(excelData);

                string FileName = "Invoice Collection Projects" + DateTime.Now.Ticks + ".xls";

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);

                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
                return Json(new { fileName = FileName });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public ActionResult GetLeaveDataReportExport(AbpProjects.LeaveApplication.Dto.GetInputDto input)
        {
            var ProjectList = _leaveapplicationRepository.GetLeaveDataReportExport(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["User Name"] = item.UserName;
                excelFields["Leave Type"] = item.LeaveTypeName;
                excelFields["From Date"] = item.FromDate.ToString();
                excelFields["To Date"] = item.ToDate.ToString();
                excelFields["Reason"] = item.Reason.ToString();
                excelFields["Leave Status Name"] = item.LeaveStatusName.ToString();

                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "UserStory" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }

        public ActionResult DailySalesActivityReportExport(GetOpportunityExportInputDto input)
        {
            var ProjectList = _iOpportunityService.DailySalesActivityReportExport(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["Person Name"] = item.PersonName;
                excelFields["Company Name"] = item.CompanyName;
                excelFields["Email Id"] = item.EmailId.ToString();
                excelFields["Mobile No"] = item.MobileNumber.ToString();
                excelFields["Total FollowUp"] = item.FollowupCount.ToString();
                excelFields["Call Category Name"] = item.CallCategoryName.ToString();

                string strInterestIn = "";
                foreach (var pr in item.ProjectType_Name)
                {
                    strInterestIn += pr.ToString() + ",";
                }
                excelFields["Interested In"] = strInterestIn;

                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "DailySalesActivity" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }

        public ActionResult OpportunityReportExport(GetOpportunityInputDto input)
        {
            var ProjectList = _iOpportunityService.OpportunityReportExport(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["Lead Owner"] = item.AssignUserName;
                excelFields["Company"] = item.CompanyName;
                excelFields["Company Beneficiary"] = item.BeneficiaryCompany;
                excelFields["Person Name"] = item.PersonName;
                //excelFields["Services"] = Convert.ToString(item.ProjectType_Name);
                string strInterestIn = "";
                foreach (var pr in item.ProjectType_Name)
                {
                    strInterestIn += pr.ToString() + ",";
                }
                excelFields["Interested In"] = strInterestIn;
                excelFields["Project Value"] = Convert.ToString(item.ProjectValue);
                excelFields["Email Id"] = item.EmailId.ToString();
                excelFields["Mobile Number"] = item.MobileNumber.ToString();
                excelFields["Call Category"] = Convert.ToString(item.CallCategoryName);
                excelFields["Closed Amount"] = item.CalllCategoryId==6 ? Convert.ToString(item.ClosedAmount) : null; 
                excelFields["Closing Date"] = item.ExpectedClosingDate != null ? string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.ExpectedClosingDate)) : null;
                //excelFields["Reason"] = Convert.ToString(item.Reason);
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "OpportunityReport" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
        public ActionResult OpportunityClosingReportExport(GetOpportunityInputDto input)
        {
            var ProjectList = _iOpportunityService.GetOpportunityClosingReportExport(input);
            var excelData = new List<Dictionary<string, string>>();
            foreach (var item in ProjectList)
            {
                Dictionary<string, string> excelFields = new Dictionary<string, string>();
                excelFields["Lead Owner"] = item.AssignUserName;
                excelFields["Company"] = item.CompanyName;
                excelFields["Company Beneficiary"] = item.BeneficiaryCompany;
                excelFields["Person Name"] = item.PersonName;
                //excelFields["Services"] = Convert.ToString(item.ProjectType_Name);
                string strInterestIn = "";
                foreach (var pr in item.ProjectType_Name)
                {
                    strInterestIn += pr.ToString() + ",";
                }
                excelFields["Interested In"] = strInterestIn;
                excelFields["Project Value"] = Convert.ToString(item.ProjectValue);
                excelFields["Email Id"] = item.EmailId.ToString();
                excelFields["Mobile Number"] = item.MobileNumber.ToString();
                excelFields["Call Category"] = Convert.ToString(item.CallCategoryName);
                excelFields["Closed Amount"] = item.CalllCategoryId == 6 ? Convert.ToString(item.ClosedAmount) : null;
                excelFields["Closing Date"] = item.ExpectedClosingDate != null ? string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(item.ExpectedClosingDate)) : null;
                //excelFields["Reason"] = Convert.ToString(item.Reason);
                excelData.Add(excelFields);
            }
            DataTable dt = ToDataTable(excelData);

            string FileName = "OpportunityReport" + DateTime.Now.Ticks + ".xls";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.ContentType = "application/ms-excel";
            Response.Charset = "UTF-8";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + FileName, sw.ToString(), Encoding.UTF8);
            return Json(new { fileName = FileName });
        }
    }
}