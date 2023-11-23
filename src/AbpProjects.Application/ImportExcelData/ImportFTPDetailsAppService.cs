using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.ImportExcelData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Abp.Authorization;
using AbpProjects.Authorization;
using AbpProjects.ManageKnowledgeCenter.Dto;

namespace AbpProjects.ImportExcelData
{
    [AbpAuthorize(PermissionNames.Pages_Import, PermissionNames.Pages_ImportList, PermissionNames.Pages_ImportExcel)]
    public class ImportFTPDetailsAppService : IImportFTPDetailsAppService
    {
        private readonly IRepository<ImportFTPDetails> _importExcelDataRepository;

        public ImportFTPDetailsAppService(IRepository<ImportFTPDetails> importExcelDataRepository)
        {
            _importExcelDataRepository = importExcelDataRepository;
        }

        #region ImportExcelData

        public async Task ImportExcelOfFTPDetails(List<importFTPDetails> inputList)
        {
            //bool result = false;
            {
                var deleteitem = _importExcelDataRepository.GetAll();
                foreach (var delitem in deleteitem)
                {
                    var id = delitem.Id;
                    await _importExcelDataRepository.DeleteAsync(id);
                }
                foreach (var item in inputList)
                {
                    //var efExsist = _importExcelDataRepository.GetAll().Where(e => e.DomainName == item.DomainName).FirstOrDefault();
                    //if (efExsist == null)
                    //{
                    var importData = item.MapTo<ImportFTPDetails>();
                    await _importExcelDataRepository.InsertAsync(importData);

                    //}
            //if (inputList != null)
                }
            }
        }

        public PagedResultDto<ImportDto> GetImportdata(GetImportExcelDto Input)
        {
            var Query = _importExcelDataRepository.GetAll().WhereIf(!Input.FilterText.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(Input.FilterText.ToLower())
            //|| ( p.HostName.ToLower().Contains(Input.FilterText.ToLower())) )
            );
            var userData = Query.OrderBy(Input.Sorting).PageBy(Input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<ImportDto>(userCount, userData.MapTo<List<ImportDto>>());

        }

        public async Task<ImportDto> getImportFTPDetail(EntityDto input)
        {
            var items = (await _importExcelDataRepository.GetAsync(input.Id)).MapTo<ImportDto>();
            return items;

        }

        public List<GetTeamListDto> GetTeams()
        {
            throw new NotImplementedException();
        }
       

        public async Task CreateFTPDetails(CreateFTPDetailsDto input)
        {
            //var res = _importExcelDataRepository.GetAll().Where(e => e.DomainName == input.DomainName).Select(x => x.Id).FirstOrDefault();
            //if (res != 0)
            //{
            //    await _importExcelDataRepository.DeleteAsync(res);

            //}
            var result = input.MapTo<ImportFTPDetails>();
            await _importExcelDataRepository.InsertAsync(result);
        }

        public async Task<ImportDto> GetServiceForEdit(int id)
        {
            
            var ftplist = (from e1 in _importExcelDataRepository.GetAll()
                         
                         where (e1.Id == id)
                         select new ImportDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             HostName = e1.HostName,
                             HostingProvider=e1.HostingProvider,
                             FtpUserName = e1.FTPUserName,
                             FtpPassword = e1.FTPPassword,
                             DbType = e1.DBType,
                             OnlineManager = e1.OnlineManager,
                             OnlineManagerHostName = e1.OnlineManagerHostName,
                             DatabaseName = e1.DatabaseName,
                             DataBaseUserName = e1.DataBaseUserName,
                             DataBasePassword = e1.DataBasePassword,
                             Storagecontainer = e1.Storagecontainer,
                             MailProvider_Host = e1.MailProvider_Host,
                             MailProvider_User = e1.MailProvider_User,
                             MailProvider_Password = e1.MailProvider_Password,
                             

                             //AdjustmentAmount = e1.AdjustmentAmount
                         })
                     .FirstOrDefault();

            return ftplist;
        }
        public async Task UpdateService(EditDto input)
        {
            try
            {
               
                var per = await _importExcelDataRepository.FirstOrDefaultAsync(input.Id);
                

                per.DomainName = input.DomainName;
                per.HostName = input.HostName;
                per.HostingProvider = input.HostingProvider;
                per.FTPUserName = input.FTPUserName;
                per.FTPPassword = input.FTPPassword;
                per.DBType = input.DBType;
                per.OnlineManager = input.OnlineManager;
                per.OnlineManagerHostName = input.OnlineManagerHostName;
                per.DatabaseName = input.DatabaseName;
                per.DataBaseUserName = input.DataBaseUserName;
                per.DataBasePassword = input.DataBasePassword;
                per.Storagecontainer = input.Storagecontainer;
                per.MailProvider_Host = input.MailProvider_Host;
                per.MailProvider_User = input.MailProvider_User;
                per.MailProvider_Password = input.MailProvider_Password;

                await _importExcelDataRepository.UpdateAsync(per);

               
                }

            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteFtp(int id)
        {

            await _importExcelDataRepository.DeleteAsync(id);


        }
        public bool CheckExist(ImportDto input)
        {
            
            var ftpexist = _importExcelDataRepository
               .GetAll().Where(e => e.DomainName == input.DomainName).Any();
            return ftpexist;
        }
        public bool ftpExsistenceById(ImportDto input)
        {
            return _importExcelDataRepository.GetAll().Where(e => e.DomainName == input.DomainName && e.Id != input.Id).Any();
        }


        #endregion
    }
}
