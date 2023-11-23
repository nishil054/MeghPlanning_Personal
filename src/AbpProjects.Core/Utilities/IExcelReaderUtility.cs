using Abp.Application.Services;
using AbpProjects.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AbpProjects.Utilities
{
    public interface IExcelReaderUtility : IApplicationService
    {
        DataSet GetDataListFromExcel(string fileName);
        //Task<string>  SaveDocument(string FolderName, HttpPostedFileBase file, List<string> Fileextension);

        //upload attachment
        Task<string> SaveAttachment(string FolderName, HttpPostedFileBase file, List<string> Fileextension);
        Task<string> SaveMultipleAttachment(string FolderName, HttpPostedFileBase file, string documentCode);
       
    }
}
