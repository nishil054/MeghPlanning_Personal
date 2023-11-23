using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.Project.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupportServices
{
    public interface ISupportAppService : IApplicationService
    {
        PagedResultDto<ListDataDto> GetServiceMgt(GetServiceInput input);
        ListResultDto<HistoryListDto> GetAllServiceHistory(int id);
        ListResultDto<ListDataDto> GetServiceName(GetServiceInput input);
        ListResultDto<ListDataDto> GetClientName(GetServiceInput input);
        ListResultDto<ListDataDto> GetEmployeeName(GetServiceInput input);


        //Get Marketing, Admin and Support users
        List<ListDto> GetUserName();
        List<ListDto> GetUserNameById(int id);
        ListResultDto<ListDataDto> GetServerName(GetServiceInput input);
        Task CreateService(InsertDataDto input);
        Task DeleteService(int id);
        Task<ListDataDto> GetServiceForEdit(int id);
        Task UpdateService(UpdateDataDto input);
        //ListResultDto<ListDataDto> GetDomainDate(GetServiceInput input);
        ListResultDto<ListDataDto> GetStorageDate(GetServiceInput input);

        ListResultDto<ListDataDto> GetemailDate(GetServiceInput input);
        ListResultDto<ListDataDto> GetHostingDate(GetServiceInput input);


        Task UpdateDashboardService(int id);
        ListResultDto<ListDataDto> GetClientNameActive(GetServiceInput input);
        ListResultDto<ListDataDto> GetEmployeeNameActive(GetServiceInput input);


        //Get Marketing, Admin and Support users
        List<ListDto> GetActiveUserName();


        ListResultDto<ListDataDto> GetTypeName(GetServiceInput input);

        Task DashboardMarkAsConfirm(UpdateDataDto input);
        ListResultDto<ListInvoiceRequestDto> GetInvoiceRequestService(int sid);
        Task CreateInvoiceRequestService(InvoiceRequestDto input);
        //Task<ListDataDto> GetServiceClearField(int id);
        //PagedResultDto<ListDataDto> ServiceFilter(GetServiceInput input);

        List<DomainListDto> GetDomainNameList(string domainname);

        Task UpdateServiceWithoutClient(UpdateServiceDto input);
    }
}
