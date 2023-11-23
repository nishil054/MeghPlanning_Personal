using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AbpProjects.Company.Dto;
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
using Abp.Notifications;
using AbpProjects.Notifications;
using Abp.Runtime.Session;
using Abp.Timing;
using System.Globalization;
using Abp.Dependency;

namespace AbpProjects.Company
{
    [AbpAuthorize(PermissionNames.Pages_DataVault, PermissionNames.Pages_DataVault_Company)]
    public class CompanyAppService : AbpProjectsApplicationModule, ICompanyAppService
    {
        private readonly IRepository<company> _companyRepository;


        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly MyService _myServiceRepository;
        private readonly IAbpSession _session;

        public CompanyAppService(IRepository<company> companyRepository,
             INotificationPublisher notificationPublisher,
          INotificationSubscriptionManager notificationSubscriptionManager,
          IAppNotifier appNotifier,
          IAbpSession session,
          MyService myServiceRepository
            )
        {
            _companyRepository = companyRepository;

            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _session = session;
            _myServiceRepository = myServiceRepository;
        }
        public bool CompanyExsistence(CreateCompanyDto input)
        {
            return _companyRepository.GetAll().Where(e => e.Beneficial_Company_Name == input.Beneficial_Company_Name).Any();
        }

        public async Task CreateCompany(CreateCompanyDto input)
        {
            var result = input.MapTo<company>();
            await _companyRepository.InsertAsync(result);
            // string message = "This is a test notification, created at " + Clock.Now;
            string message = "New  company added, created at " + Clock.Now;
            string severity = "success";
            // await _appNotifier.Publish_AddCountry(message);

            //vikas
            //send message test msg to current user createCompany
            await _appNotifier.SendMessageAsync(
                _session.ToUserIdentifier(),
                message,
                severity.ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
                );

            //end vikas 

            // await _appNotifier.SendMessageAsync("TestNotification", null, null,NotificationSeverity.Info,Convert.ToInt32(_session.UserId) ,null,null);



        }
        public PagedResultDto<CompanyDto> GetCompanydata(GetCompanyDto Input)
        {
            var Query = _companyRepository.GetAll();
            var userData = Query.OrderBy(Input.Sorting).PageBy(Input).ToList();
            var userCount = Query.Count();
            return new PagedResultDto<CompanyDto>(userCount, userData.MapTo<List<CompanyDto>>());

        }
        public List<CompanyDto> GetCompany()
        {
            var result = (from a in _companyRepository.GetAll()
                          select new CompanyDto
                          {
                              Id = a.Id,
                              Beneficial_Company_Name = a.Beneficial_Company_Name,
                          }).ToList();
            return result;
        }
        public async Task<CompanyDto> GetDataById(EntityDto input)
        {
            var c = (await _companyRepository.GetAsync(input.Id)).MapTo<CompanyDto>();
            return c;
        }
        public async Task UpdateCompany(EditCompanyDto input)
        {

            var Tests = await _companyRepository.GetAsync(input.Id);

            Tests.Beneficial_Company_Name = input.Beneficial_Company_Name;


            await _companyRepository.UpdateAsync(Tests);
        }
        public bool CompanyExsistenceById(CreateCompanyDto input)
        {
            return _companyRepository.GetAll().Where(e => e.Beneficial_Company_Name == input.Beneficial_Company_Name && e.Id != input.Id).Any();
        }
        public async Task DeleteCompany(EntityDto input)
        {
            await _companyRepository.DeleteAsync(input.Id);
        }
        public PagedResultDto<CompanyDto> GetCompanyList(GetCompanyDto input)
        {
            var cc = _companyRepository.GetAll()
                .WhereIf(!input.Beneficial_Company_Name.IsNullOrEmpty(), p => p.Beneficial_Company_Name.ToLower().Contains(input.Beneficial_Company_Name.ToLower())
               );
            var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccCount = cc.Count();
            //return await Task.FromResult(cc.ToList());
            return new PagedResultDto<CompanyDto>(ccCount, ccData.MapTo<List<CompanyDto>>());
        }
    }
}
