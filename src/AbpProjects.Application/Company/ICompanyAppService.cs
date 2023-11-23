using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Company.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Company
{
  public interface ICompanyAppService : IApplicationService
    {
        Task CreateCompany(CreateCompanyDto input);
        bool CompanyExsistence(CreateCompanyDto input);
        bool CompanyExsistenceById(CreateCompanyDto input);
        List<CompanyDto> GetCompany();
        Task<CompanyDto> GetDataById(EntityDto input);
        Task UpdateCompany(EditCompanyDto input);
        Task DeleteCompany(EntityDto input);
        PagedResultDto<CompanyDto> GetCompanydata(GetCompanyDto Input);
        PagedResultDto<CompanyDto> GetCompanyList(GetCompanyDto input);
    }
}
