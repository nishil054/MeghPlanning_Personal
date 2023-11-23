using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ExpenseCategoryService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseCategoryServices
{
    public interface IExpcategoryAppService: IApplicationService
    {
        bool ExpenseCategoryExsistence(CreateDto input);
        Task ExpenseCreateCategory(CreateDto input);
        PagedResultDto<ListDto> GetExpenseCategory(GetExpensecategoryInput Input);
        Task DeleteExpenseCategory(EntityDto input);
        Task<ListDto> GetExpenseCategoryForEdit(EntityDto input);
        bool ExpenseCategoryExsistenceById(EditDto input);
        Task UpdateExpenseCategory(EditDto input);
        Task<ListDto> GetExpenseCategoryForDetail(EntityDto input);
    }
}
