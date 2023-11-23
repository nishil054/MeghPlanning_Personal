using Abp.Domain.Repositories;
using AbpProjects.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N.EntityFramework.Extensions;
using AbpProjects.BulkData.Dto;
using System.Data;
using System.IO;
using ExcelDataReader;
using AbpProjects.Utilities;
using System.Web;
using Abp.Authorization;
using AbpProjects.Authorization;

namespace AbpProjects.BulkData
{
    [AbpAuthorize(PermissionNames.Pages_Utility)]
    public class BulkDataAppService : IBulkDataAppService
    {
        private readonly IRepository<bulkmaster> _bulkdataRepository;
        public readonly IExcelReaderUtility _excelReader;
        public BulkDataAppService(IRepository<bulkmaster> bulkdataRepository,
            IExcelReaderUtility excelReader)
        {
            _bulkdataRepository = bulkdataRepository;
            _excelReader = excelReader;

        }
        public async Task SaveBulkDataInDB(InsertBulkData input)
        {
            List<bulkmaster> importBulk_Items = input.BulkItemsData;
            //Data save in database
            //foreach(var item in importBulk_Items)
            //{
            //    var result = await _bulkdataRepository.InsertAsync(item);
            //}
            var dbcontext = new AbpProjectsDbContext();
            {
                try
                {
                  //  dbcontext.BulkInsert<bulkmaster>(importBulk_Items);
                }
                catch (Exception ex)
                { };
            }
        }

        public DataSet GetDataFromExcel(string fileName)
        {
            var data = _excelReader.GetDataListFromExcel(fileName);
            return data;
        }
            
        //public Task<string> SaveDocument(string FolderName, HttpPostedFileBase file, List<string> extension)
        //{

        //    var filename = _excelReader.SaveDocument(FolderName, file, extension);
        //    return filename;
        //}

        
    }
}
