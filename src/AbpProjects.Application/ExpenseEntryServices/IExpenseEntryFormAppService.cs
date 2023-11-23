using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ExpenseEntryServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseEntryServices
{
    public interface IExpenseEntryFormAppService : IApplicationService
    {
        Task CreateExpenseEntry(List<InsertDto> input, DateTime date);
        ListResultDto<ListDto> GetExpenseEntry(DateTime monthyr);
        //ListResultDto<ListDto> GetExpenseEntryReport();
    }
}
