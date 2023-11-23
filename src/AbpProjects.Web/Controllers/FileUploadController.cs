using System.Linq;
using Abp.UI;
using Abp.Web.Models;
using AbpProjects.ImportExcelData;
using AbpProjects.ImportExcelData.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using AbpProjects.ManageKnowledgeCenter.Dto;
using AbpProjects.ManageKnowledgeCenter;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.ImportUserStoryData.Dto;
using AbpProjects.ImportUserStoryData;
using AbpProjects.Utilities;
using System.Net.Http;
using AbpProjects.Document.Dto;
using System.Web.Hosting;
using AbpProjects.FileUploadByDirective;
using AbpProjects.Opportunities;
using AbpProjects.BulkData.Dto;
using AbpProjects.OpportunityAppServices;
using Abp.Runtime.Session;
using AbpProjects.Company;

namespace AbpProjects.Web.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload

        private readonly IImportFTPDetailsAppService _importExcelRepository;
        private readonly IImportUserStoryDetailsAppService _importUserStoryDataRepository;
        private readonly IOpportunityService _bulkOpportunityDataRepository;
        private readonly IRepository<KnowledgeDocuments> _knowledgedocumentRepository;
        public readonly IExcelReaderUtility _excelReader;
        private readonly IRepository<Documentchild> _documentchildRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<company> _companyRepository;

        public FileUploadController(IImportFTPDetailsAppService importExcelRepository,
            IImportUserStoryDetailsAppService importUserStoryDataRepository,
            IRepository<KnowledgeDocuments> knowledgedocumentRepository, IExcelReaderUtility excelReader, IRepository<Documentchild> documentchildRepository,
            IOpportunityService bulkOpportunityDataRepository,
            IAbpSession abpSession, IRepository<company> companyRepository)
        {
            _importExcelRepository = importExcelRepository;
            _importUserStoryDataRepository = importUserStoryDataRepository;
            _knowledgedocumentRepository = knowledgedocumentRepository;
            _excelReader = excelReader;
            _documentchildRepository = documentchildRepository;
            _bulkOpportunityDataRepository = bulkOpportunityDataRepository;
            _abpSession = abpSession;
            _companyRepository = companyRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadDocumentAttachments(HttpPostedFileBase Files)
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException("");//L("ProfilePicture_Change_Error")
                }

                var file = Request.Files[0];

                var acceptedFormats = new List<string> { ".pdf", ".jpg", ".jpeg", ".doc", ".docx", ".txt", ".xls", ".xlsx" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                var fileInfo = new FileInfo(file.FileName);
                if (!acceptedFormats.Contains(fileExt.ToLower()))
                {

                    // throw new UserFriendlyException("DocumentsFill");

                }
                string tempFileName = "";
                tempFileName = Path.GetFileNameWithoutExtension(file.FileName.ToString().Replace(" ", "_")) + "_" + DateTime.Now.ToString("ddMMyyHHmmssffffff") + fileInfo.Extension;
                //tempFileName = Path.GetFileNameWithoutExtension(file.FileName) + fileInfo.Extension;

                if (!Directory.Exists(Path.Combine(Server.MapPath("~/UserFiles/Documents/"))))
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/UserFiles/Documents/")));
                }


                var ServerSavePath = Path.Combine(Server.MapPath("~/UserFiles/Documents/") + tempFileName);
                file.SaveAs(ServerSavePath);


                return Json(new AjaxResponse(new
                {
                    fileName = tempFileName,
                    path = ""

                }));

            }

            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));

            }
        }

        #region ImportExcelData

        public async Task<ActionResult> ImportExcelData(HttpPostedFileBase file)
        {
            ImportExcelData.Dto.ExcelValidationList excel = new ImportExcelData.Dto.ExcelValidationList();
            excel.isnullcolumn = false;
            string rows = "row ";
            var rowcount = new List<string>();
            //var nullcolumnscount = new List<string>();
            //var notexsistcolumnscount = new List<string>();
            var errorcount = new List<string>();

            if (file != null)
            {
                var file_Name = "Import_" + DateTime.Now.Ticks.ToString() + ".XLSX";

                string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\ImportExcelData\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                                                                                                                                               //var fileName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "userfiles\\Import_Excel\\" , file_Name);//_appFolders.TempFileDownloadFolder
                                                                                                                                               // string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\Import_Excel\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                file.SaveAs(fileName);
                DataTable dt = new DataTable();
                DataSet ds = GetDataFromExcel(fileName);  // Excel Data Reader
                dt = ds.Tables[0];

                List<importFTPDetails> Bulkitemdata = new List<importFTPDetails>();

                try
                {
                    Bulkitemdata = (from DataRow row in dt.Rows
                                    select new importFTPDetails
                                    {
                                        DomainName = Convert.ToString(row["Domain Name"]),
                                        HostName = Convert.ToString(row["Host Name"]),
                                        HostingProvider = Convert.ToString(row["Hosting Provider"]),
                                        FTPUserName = Convert.ToString(row["FTPUserName"]),
                                        FTPPassword = Convert.ToString(row["FTPPassword"]),
                                        DBType = Convert.ToString(row["DBType"]),
                                        OnlineManager = Convert.ToString(row["OnlineManager"]),
                                        OnlineManagerHostName = Convert.ToString(row["OnlineManager HostName"]),
                                        DatabaseName = Convert.ToString(row["Database Name"]),
                                        DataBaseUserName = Convert.ToString(row["DataBase UserName"]),
                                        DataBasePassword = Convert.ToString(row["DataBase Password"]),
                                        Storagecontainer = Convert.ToString(row["Storage container"]),
                                        MailProvider_Host = Convert.ToString(row["MailProvider_Host"]),
                                        MailProvider_User = Convert.ToString(row["MailProvider_User"]),
                                        MailProvider_Password = Convert.ToString(row["MailProvider_Password"])
                                    }).ToList();

                    if (excel.isnullcolumn == false)
                    {
                        await _importExcelRepository.ImportExcelOfFTPDetails(Bulkitemdata);
                        return Json(new { success = true, excel }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {

                    return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
                }


                //return Json(new { success = true, excel }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
            }
        }

        public DataSet GetDataFromExcel(string fileName)
        {
            System.IO.FileStream stream1 = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read);
            using (var stream = stream1)
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                //using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                //using (var reader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {

                        // Gets or sets a value indicating whether to set the DataColumn.DataType 
                        // property in a second pass.
                        UseColumnDataType = true,

                        // Gets or sets a callback to obtain configuration options for a DataTable. 
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {

                            // Gets or sets a value indicating the prefix of generated column names.
                            EmptyColumnNamePrefix = "Column",

                            // Gets or sets a value indicating whether to use a row from the 
                            // data as column names.
                            UseHeaderRow = true,

                            // Gets or sets a callback to determine which row is the header row. 
                            // Only called when UseHeaderRow = true.
                            ReadHeaderRow = (rowReader) =>
                            {
                                // F.ex skip the first row and use the 2nd row as column headers:
                                //rowReader.Read();
                            },

                            // Gets or sets a callback to determine whether to include the 
                            // current row in the DataTable.
                            FilterRow = (rowReader) =>
                            {
                                return true;
                            },

                            // Gets or sets a callback to determine whether to include the specific
                            // column in the DataTable. Called once per column after reading the 
                            // headers.
                            FilterColumn = (rowReader, columnIndex) =>
                            {
                                return true;
                            }
                        }
                    });
                    return result;
                    // The result of each spreadsheet is in result.Tables
                }
            }
        }
        public DataSet GetDataFromCSVFormat(string fileName)
        {
            System.IO.FileStream stream1 = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read);
            using (var stream = stream1)
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                //using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                //using (var reader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {

                        // Gets or sets a value indicating whether to set the DataColumn.DataType 
                        // property in a second pass.
                        UseColumnDataType = true,

                        // Gets or sets a callback to obtain configuration options for a DataTable. 
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {

                            // Gets or sets a value indicating the prefix of generated column names.
                            EmptyColumnNamePrefix = "Column",

                            // Gets or sets a value indicating whether to use a row from the 
                            // data as column names.
                            UseHeaderRow = true,

                            // Gets or sets a callback to determine which row is the header row. 
                            // Only called when UseHeaderRow = true.
                            ReadHeaderRow = (rowReader) =>
                            {
                                // F.ex skip the first row and use the 2nd row as column headers:
                                //rowReader.Read();
                            },

                            // Gets or sets a callback to determine whether to include the 
                            // current row in the DataTable.
                            FilterRow = (rowReader) =>
                            {
                                return true;
                            },

                            // Gets or sets a callback to determine whether to include the specific
                            // column in the DataTable. Called once per column after reading the 
                            // headers.
                            FilterColumn = (rowReader, columnIndex) =>
                            {
                                return true;
                            }
                        }
                    });
                    return result;
                    // The result of each spreadsheet is in result.Tables
                }
            }
        }

        #endregion

        #region Manage Knowledge Center

        [HttpPost]
        public JsonResult UploadMultipleAttachment(int knowlwdgecenterId)
        {
            try
            {
                //Check input
                List<ProjectAttachmentView> FilenameList = new List<ProjectAttachmentView>();
                ProjectAttachmentView fileData = new ProjectAttachmentView();

                HttpFileCollectionBase files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase filedata = files[i];
                    var file = filedata;

                    string FileName = filedata.FileName.Replace(" ", "");
                    fileData.FileName = FileName;

                    //var acceptedFormats = new List<string> { ".png", ".pdf", ".jpg", ".jpeg", ".xls", ".xlsx", ".bmp" };

                    //var fileInfo = new FileInfo(file.FileName);
                    //if (!acceptedFormats.Contains(fileInfo.Extension.ToLower()))
                    //{
                    //    throw new ApplicationException("Only .png,.pdf,.jpg,.jpeg,xls,.xlsx,.bmp files are allowed to upload.");
                    //}

                    var file_Name = knowlwdgecenterId + "_" + FileName;
                    fileData.NewFileName = file_Name;
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/userfiles/KnowledgeCenter/"))))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/userfiles/KnowledgeCenter/")));
                    }
                    string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\KnowledgeCenter\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                    file.SaveAs(filePath);

                    fileData.FilePath = filePath;
                    FilenameList.Add(fileData);

                    KnowledgeDocuments doc = new KnowledgeDocuments();
                    doc.DocumentName = fileData.NewFileName;
                    doc.FileName = fileData.FileName;
                    doc.FilePath = fileData.FilePath;
                    doc.KnowledgeCenterId = knowlwdgecenterId;
                    _knowledgedocumentRepository.InsertAsync(doc);
                }
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException("");//L("ProfilePicture_Change_Error")
                }
                return Json(new AjaxResponse(new
                {
                    FileObject = FilenameList
                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        #endregion


        public async Task<ActionResult> SaveOpportunityData(HttpPostedFileBase file, string opportunity)
        {
            try
            {
                ImportExcelData.Dto.ExcelValidationList excel = new ImportExcelData.Dto.ExcelValidationList();
                excel.isnullcolumn = false;
                string rows = "row ";
                var rowcount = new List<string>();
                //var nullcolumnscount = new List<string>();
                //var notexsistcolumnscount = new List<string>();
                var errorcount = new List<string>();

                if (file != null)
                {

                    var file_Name = "BulkData_" + DateTime.Now.Ticks.ToString() + ".xlsx";
                    string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "Temp\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                    file.SaveAs(fileName);
                    List<string> extension = new List<string> { ".xls", "xlsx" };
                    //    var fileName = await _bulkDataRepository.SaveDocument("Temp", file, extension);
                    DataTable dt = new DataTable();
                    // DataSet ds = _bulkDataRepository.GetDataFromExcel(fileName);
                    DataTable dtbl = new DataTable();
                    DataSet ds = GetDataFromExcel(fileName);

                    dt = ds.Tables[0];
                    int i = 0;
                    int CompanyBeneficiary = 0;
                    int Opportunityowner = 0;
                    if (opportunity == "0")
                    {
                        Opportunityowner = (int)_abpSession.GetUserId();
                    }

                    List<Opportunity> Bulkitemdata = new List<Opportunity>();
                    DataTable Customdt = new DataTable();

                    Customdt.Columns.Add("CompanyName", typeof(System.String));
                    Customdt.Columns.Add("CompanyBeneficiary", typeof(System.String));
                    Customdt.Columns.Add("PersonName", typeof(System.String));
                    Customdt.Columns.Add("EmailId", typeof(System.String));
                    Customdt.Columns.Add("MobileNo", typeof(System.String));

                    foreach(DataRow row in dt.Rows)
                    {
                        if (row.ItemArray[1] != null)
                        {
                            var company = Convert.ToString(row.ItemArray[1]);
                            CompanyBeneficiary = _companyRepository.GetAll().Where(x => x.Beneficial_Company_Name == company).Select(x => x.Id).FirstOrDefault();
                            if (CompanyBeneficiary != 0)
                            {
                                Opportunity obj = new Opportunity();
                                obj.CompanyName = Convert.ToString(row.ItemArray[0]);
                                obj.BeneficiaryCompanyId = CompanyBeneficiary;
                                obj.PersonName = Convert.ToString(row.ItemArray[2]);
                                obj.EmailId = Convert.ToString(row.ItemArray[3]);
                                obj.MobileNumber = Convert.ToString(row.ItemArray[4]);
                                obj.OpportunityOwner = Opportunityowner;
                                Bulkitemdata.Add(obj);
                            }
                        }
                    }

                    //List<Opportunity> Bulkitemdata = new List<Opportunity>();
                    try
                    {

                        //Bulkitemdata = (from DataRow row in dt.Rows

                        //                select new Opportunity
                        //                {
                        //                    CompanyName = Convert.ToString(row["CompanyName"]),
                        //                    BeneficiaryCompanyId = Convert.ToInt32(row["CompanyBeneficiary"]),
                        //                    PersonName = Convert.ToString(row["PersonName"]),
                        //                    EmailId = Convert.ToString(row["EmailId"]),
                        //                    MobileNumber = Convert.ToString(row["MobileNo"]),
                        //                    OpportunityOwner = Opportunityowner
                        //                }).ToList();

                        if (excel.isnullcolumn == false)
                        {
                            InsertOpportunityBulkData insertitem = new InsertOpportunityBulkData();
                            insertitem.BulkOpportunityItemsData = Bulkitemdata;
                            await _bulkOpportunityDataRepository.SaveBulkDataInDB(insertitem);
                            return Json(new { success = true, excel }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
                        }



                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); };

        }
        #region ImportUserStoryExcelData

        public async Task<ActionResult> ImportUserStoryExcelData(HttpPostedFileBase file, string projectid)
        {
            BulkData.Dto.ExcelValidationList excel = new BulkData.Dto.ExcelValidationList();
            excel.isnullcolumn = false;
            string rows = "row ";
            var rowcount = new List<string>();
            //var nullcolumnscount = new List<string>();
            //var notexsistcolumnscount = new List<string>();
            var errorcount = new List<string>();

            if (file != null)
            {
                var file_Name = "Import_" + DateTime.Now.Ticks.ToString() + ".XLSX";

                string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\ImportUserStoryData\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                                                                                                                                                   //var fileName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "userfiles\\Import_Excel\\" , file_Name);//_appFolders.TempFileDownloadFolder
                                                                                                                                                   // string fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "userfiles\\Import_Excel\\" + file_Name; //System.Configuration.ConfigurationManager.AppSettings["FileToImport"].ToString();
                file.SaveAs(fileName);
                DataTable dt = new DataTable();
                DataSet ds = GetDataFromUserStoryExcel(fileName);  // Excel Data Reader
                dt = ds.Tables[0];

                List<importUserStoryDetails> Bulkitemdata = new List<importUserStoryDetails>();

                try
                {
                    Bulkitemdata = (from DataRow row in dt.Rows
                                    select new importUserStoryDetails
                                    {
                                        UserStory = Convert.ToString(row["User Story"]),
                                        ProjectId = Convert.ToInt32(projectid),
                                        DeveloperHours = Convert.ToDecimal(row["Developer Hours"]),
                                        ExpectedHours = Convert.ToDecimal(row["Expected Hours"]),
                                        status = 0
                                        //EmployeeId = 0
                                        //ActualHours = Convert.ToDecimal(row["Actual Hours"]),   
                                    }).ToList();

                    if (excel.isnullcolumn == false)
                    {
                        await _importUserStoryDataRepository.ImportUserDataDetails(Bulkitemdata);
                        return Json(new { success = true, excel }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {

                    return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
                }


                //return Json(new { success = true, excel }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, excel }, JsonRequestBehavior.AllowGet);
            }
        }


        public DataSet GetDataFromUserStoryExcel(string fileName)
        {
            System.IO.FileStream stream1 = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read);
            using (var stream = stream1)
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                //using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                //using (var reader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {

                        // Gets or sets a value indicating whether to set the DataColumn.DataType 
                        // property in a second pass.
                        UseColumnDataType = true,

                        // Gets or sets a callback to obtain configuration options for a DataTable. 
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {

                            // Gets or sets a value indicating the prefix of generated column names.
                            EmptyColumnNamePrefix = "Column",

                            // Gets or sets a value indicating whether to use a row from the 
                            // data as column names.
                            UseHeaderRow = true,

                            // Gets or sets a callback to determine which row is the header row. 
                            // Only called when UseHeaderRow = true.
                            ReadHeaderRow = (rowReader) =>
                            {
                                // F.ex skip the first row and use the 2nd row as column headers:
                                //rowReader.Read();
                            },

                            // Gets or sets a callback to determine whether to include the 
                            // current row in the DataTable.
                            FilterRow = (rowReader) =>
                            {
                                return true;
                            },

                            // Gets or sets a callback to determine whether to include the specific
                            // column in the DataTable. Called once per column after reading the 
                            // headers.
                            FilterColumn = (rowReader, columnIndex) =>
                            {
                                return true;
                            }
                        }
                    });
                    return result;
                    // The result of each spreadsheet is in result.Tables
                }
            }
        }

        #endregion

        //upload file using directive
        [HttpPost]
        public async Task<JsonResult> UploadFiles(string documentCode, string folderName)
        {
            try
            {
                List<string> FilenameList = new List<string>();
                DocumentAttachmentView fileData = new DocumentAttachmentView();
                var filename = new List<string>();
                var filepath = new List<string>();

                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase filedata = files[i];
                    var file = filedata;

                    filename.Add(filedata.FileName);

                    var tempFileName = await _excelReader.SaveMultipleAttachment(folderName, file, documentCode);
                    var tempFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "UserFiles\\" + "\\" + folderName, tempFileName.ToString());

                    filepath.Add(tempFilePath);
                    fileData.FileName = filename;
                    fileData.FilePath = filepath;

                    //data insertion in child table
                    Documentchild doc = new Documentchild();
                    doc.FileName = fileData.FileName[i];
                    doc.DocumentId = Convert.ToInt32(documentCode);
                    await _documentchildRepository.InsertAsync(doc);

                }

                return Json(new AjaxResponse(new
                {
                    FileObject = fileData

                }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));

            }
        }
    }

}