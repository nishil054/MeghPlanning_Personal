using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.FinancialYear.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FinancialYear
{
    public interface IFinancialYearAppService: IApplicationService
    {
        //ListResultDto<FinancialYearDto> GetYear()
        PagedResultDto<FinancialYearListDto> GetFinancialYearList(GetFinanceYearDto Input);
        bool FinancialYearExsistence(GetFinancialYearDto input);
        bool FinancialYearExsistenceByid(GetFinancialYearDto input);
        List<FinancialYearDto> GetFinancialYear();
        Task CreateFinancialYear(CreateFinancialyearDto input);
        Task<FinancialYearListDto> GetFinancialDataById(EntityDto input);
        Task UpdateFinancialYear(EditFinancialYearDto input);
        Task DeleteFinancialYear(EntityDto input);
    }
}
