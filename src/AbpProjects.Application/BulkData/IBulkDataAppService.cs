using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbpProjects.BulkData.Dto;
using System.Data;
using System.Web;

namespace AbpProjects.BulkData
{
    public interface IBulkDataAppService : IApplicationService
    {
        Task SaveBulkDataInDB(InsertBulkData input);
        DataSet GetDataFromExcel(string fileName);

       //Task<string> SaveDocument(string FolderName, HttpPostedFileBase file,List<string> extension);

    }
}
