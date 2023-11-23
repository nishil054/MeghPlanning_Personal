using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Abp.Configuration;
using Abp.UI;
using AbpProjects.Configuration;
using AbpProjects.Utilities;
using AbpProjects.Utilities.DTO;
using ExcelDataReader;
using net.openstack.Core.Domain;
using net.openstack.Providers.Rackspace;
using System.Text.RegularExpressions;

namespace AbpProjects.Utilities
{
    public class ExcelReaderUtility : IExcelReaderUtility
    {
        private readonly ISettingManager _SettingManager;
        public ExcelReaderUtility(ISettingManager settingManager)
        {
            _SettingManager = settingManager;

        }

        public DataSet GetDataListFromExcel(string fileName)
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

        
        public async Task<string> SaveAttachment(string FolderName, HttpPostedFileBase file, List<string> FileExtension)
        {
                string tempFileName = "";
                try
                {
                    if (file != null)
                    {
                        var acceptedFormats = new List<string>();
                        acceptedFormats = FileExtension;

                        //remove space,special character from filename
                        string FileName = Regex.Replace(file.FileName.Trim(), "[^A-Za-z0-9_.]+", ""); 

                        var fileInfo = new FileInfo(FileName);
                    //check file format 
                    if (acceptedFormats != null)
                    {
                        if (!acceptedFormats.Contains(fileInfo.Extension.ToLower()))
                        {
                            throw new ApplicationException("Invalid File Format.");
                        }
                    }
                    tempFileName = Path.GetFileNameWithoutExtension(FileName) + "_" + DateTime.Now.ToString("ddMMyyHHmmssffffff") + fileInfo.Extension;

                        if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~\\UserFiles\\" + "\\" + FolderName)))
                        {
                            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\UserFiles\\" + "\\" + FolderName));
                        }

                        var tempFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "UserFiles\\" + "\\" + FolderName, tempFileName);
                        file.SaveAs(tempFilePath);

                        var cdnflag = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNFlag);
                        if (cdnflag == "true")
                        {

                            try
                            {
                                GC.Collect();
                                string username = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNUserName);
                                string api_key = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNKey);
                                string chosenContainer = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNContainer);
                                string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + FolderName;
                                var cloudIdentity = new CloudIdentity() { APIKey = api_key, Username = username };
                                var cloudFilesProvider = new CloudFilesProvider(cloudIdentity);

                                cloudFilesProvider.CreateObjectFromFile(chosenContainer, HttpContext.Current.Request.PhysicalApplicationPath + "/UserFiles/" + FolderName + "/" + tempFileName, FolderName + "/" + tempFileName);
                                System.IO.FileInfo backupfile = new System.IO.FileInfo(HttpContext.Current.Request.PhysicalApplicationPath + "/UserFiles/" + FolderName + "/" + tempFileName);
                                bool fileexists = backupfile.Exists;
                                if (fileexists)
                                {
                                    GC.Collect();
                                    try
                                    {
                                        backupfile.Delete();
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            catch
                            {

                            }
                        }


                        return tempFileName;
                    }

                }
                catch (UserFriendlyException ex)
                {

                }

                return tempFileName;
            }

        //file check with extension
        //public async Task<string> SaveDocument(string FolderName, HttpPostedFileBase file,  List<string> Fileextension)
        //{
        //    string tempFileName = "";
        //    try
        //    {
        //        if (file != null)
        //        {
        //            var acceptedFormats = new List<string>();
        //            acceptedFormats = Fileextension;
        //            //remove space ffrom filename
        //            string FileName = file.FileName.Replace(" ", "");

        //            var fileInfo = new FileInfo(FileName);
        //            //check file format 
        //            if (acceptedFormats != null)
        //            {
        //                if (!acceptedFormats.Contains(fileInfo.Extension.ToLower()))
        //                {
        //                    throw new ApplicationException("Invalid File Format.");
        //                }
        //            }
        //            tempFileName = Path.GetFileNameWithoutExtension(FileName) + "_" + DateTime.Now.ToString("ddMMyyHHmmssffffff") + fileInfo.Extension;

        //            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~\\userfiles\\" + "\\" + FolderName)))                                                                                                   //var serverpath = Path.Combine(Server.MapPath("~"), "images", tempFileName);
        //            {
        //                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\userfiles\\" + "\\" + FolderName));
        //            }

        //            var tempFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "userfiles\\" + "\\" + FolderName, tempFileName);
        //            file.SaveAs(tempFilePath);
        //            var cdnflag = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNFlag);
        //            if (cdnflag=="true")
        //            {

        //                try
        //                {
        //                    GC.Collect();
        //                    string username = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNUserName);
        //                    string api_key = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNKey);
        //                    string chosenContainer = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNContainer);
        //                    string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + FolderName;
        //                    var cloudIdentity = new CloudIdentity() { APIKey = api_key, Username = username };
        //                    var cloudFilesProvider = new CloudFilesProvider(cloudIdentity);

        //                    cloudFilesProvider.CreateObjectFromFile(chosenContainer, HttpContext.Current.Request.PhysicalApplicationPath + "/userfiles/" + FolderName + "/" + tempFileName, FolderName + "/" + tempFileName);
        //                    System.IO.FileInfo backupfile = new System.IO.FileInfo(HttpContext.Current.Request.PhysicalApplicationPath + "/userfiles/" + FolderName + "/" + tempFileName);
        //                    bool fileexists = backupfile.Exists;
        //                    if (fileexists)
        //                    {
        //                        GC.Collect();
        //                        try
        //                        {
        //                            backupfile.Delete();
        //                        }
        //                        catch
        //                        {

        //                        }
        //                    }
        //                }
        //                catch
        //                {

        //                }
        //            }

        //            return tempFileName;
        //        }
              
        //    }
        //    catch (UserFriendlyException ex)
        //    {

        //    }

        //    return tempFileName;
        //}

        //file check without extension
        public async Task<string> SaveMultipleAttachment(string FolderName, HttpPostedFileBase file, string documentCode)
        {
            string tempFileName = "";
            try
            {
                if (file != null)
                {

                    //remove space,special character from filename
                    string FileName = Regex.Replace(file.FileName.Trim(), "[^A-Za-z0-9_.]+", "");

                    var fileInfo = new FileInfo(FileName);

                    tempFileName = documentCode + "_" + Path.GetFileNameWithoutExtension(FileName) + "_" + DateTime.Now.ToString("ddMMyyHHmmssffffff") + fileInfo.Extension;

                    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~\\UserFiles\\" + "\\" + FolderName)))
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\UserFiles\\" + "\\" + FolderName));
                    }

                    var tempFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "UserFiles\\" + "\\" + FolderName, tempFileName);
                    file.SaveAs(tempFilePath);

                    var cdnflag = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNFlag);
                    if (cdnflag == "true")
                    {

                        try
                        {
                            GC.Collect();
                            string username = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNUserName);
                            string api_key = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNKey);
                            string chosenContainer = await _SettingManager.GetSettingValueAsync(AppSettings.CDNSettingNames.CDNContainer);
                            string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "/" + FolderName;
                            var cloudIdentity = new CloudIdentity() { APIKey = api_key, Username = username };
                            var cloudFilesProvider = new CloudFilesProvider(cloudIdentity);

                            cloudFilesProvider.CreateObjectFromFile(chosenContainer, HttpContext.Current.Request.PhysicalApplicationPath + "/UserFiles/" + FolderName + "/" + tempFileName, FolderName + "/" + tempFileName);
                            System.IO.FileInfo backupfile = new System.IO.FileInfo(HttpContext.Current.Request.PhysicalApplicationPath + "/UserFiles/" + FolderName + "/" + tempFileName);
                            bool fileexists = backupfile.Exists;
                            if (fileexists)
                            {
                                GC.Collect();
                                try
                                {
                                    backupfile.Delete();
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch
                        {

                        }
                    }


                    return tempFileName;
                }

            }
            catch (UserFriendlyException ex)
            {

            }

            return tempFileName;
        }
    }
}
