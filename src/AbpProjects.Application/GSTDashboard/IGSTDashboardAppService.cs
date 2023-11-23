using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Company.Dto;
using AbpProjects.GSTDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.GSTDashboard
{
    public interface IGSTDashboardAppService: IApplicationService
    {
        List<GetGstDataListDto> GetGstDataList(GetGstDataDto input);
        List<GetMonthNameDto> GetMonth();
        Task<GetGstDataListDto> GetDataById(EntityDto input);
        Task UpdateGstData(EditGstDataDto input);
        Task DeleteGstData(EntityDto input);
      Task CreateService(CreateGstDataDto input);
        Task UpdateStatuslist(UpdateStatus input);
        decimal GetSum(int? month, int? company, int? finacialyearid);
        List<CompanyDto> GetCompany();
        int GetEndYear(int? finacialyearid);
        string GetStartYear(int? finacialyearid);
    }
}
