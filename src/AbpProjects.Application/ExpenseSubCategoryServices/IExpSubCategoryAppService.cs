using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.ExpenseSubCategoryServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseSubCategoryServices
{
    public interface IExpSubCategoryAppService : IApplicationService
    {
        bool ExpenseSubCategoryExsistence(InsertData input);
        Task ExpenseCreateSubCategory(InsertData input);
        PagedResultDto<ListData> GetExpenseSubCategory(GetExpenseSubcategoryInput Input);
        Task DeleteExpenseSubCategory(EntityDto input);
        Task<ListData> GetExpenseSubCategoryForEdit(EntityDto input);
        bool ExpenseSubCategoryExsistenceById(EditData input);
        Task UpdateExpenseSubCategory(EditData input);
        Task<ListData> GetExpenseSubCategoryForDetail(EntityDto input);
        ListResultDto<ListData> GetCategory(GetExpenseSubcategoryInput input);
        Task<ListData> GetExpenseCategoryForDetail(EntityDto input);

    }
}
