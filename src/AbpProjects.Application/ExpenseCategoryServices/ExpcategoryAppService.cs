using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AbpProjects.ExpenseCategories;
using AbpProjects.ExpenseCategoryService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseCategoryServices
{
    public class ExpcategoryAppService: AbpProjectsApplicationModule, IExpcategoryAppService
    {
        private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;
        private readonly IRepository<ExpenseSubcategory> _expenseSubCategoryRepository;
        public ExpcategoryAppService(IRepository<ExpenseCategory> expenseCategoryRepository, IRepository<ExpenseSubcategory> expenseSubCategoryRepository)
        {

            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseSubCategoryRepository = expenseSubCategoryRepository;
        }
        public bool ExpenseCategoryExsistence(CreateDto input)
        {

            var items = _expenseCategoryRepository.GetAll().Where(e => e.Category == input.Category).Any();
            return items;
        }

        public async Task ExpenseCreateCategory(CreateDto input)
        {
            var category = input.MapTo<ExpenseCategory>();
            await _expenseCategoryRepository.InsertAsync(category);
        }

        public PagedResultDto<ListDto> GetExpenseCategory(GetExpensecategoryInput Input)
        {
            try
            {
                
                var cate = _expenseCategoryRepository.GetAll()
                            .WhereIf(!Input.CategoryFilter.IsNullOrEmpty(), p => p.Category.ToLower().Contains(Input.CategoryFilter.ToLower()));
              
                   //.OrderByDescending(p => p.Id).ToList();
                var categoryPageBy = cate.OrderBy(Input.Sorting).PageBy(Input).ToList();

                var totalcount = cate.Count();

                return new PagedResultDto<ListDto>(totalcount, categoryPageBy.MapTo<List<ListDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task DeleteExpenseCategory(EntityDto input)
        {
            var items = _expenseSubCategoryRepository.GetAll().Where(e => e.CategoryId == input.Id).Any();
            if(items != false)
            {
                throw new UserFriendlyException("This Category can not be Deleted its Exist in subcategory");
            }
            else
            {
                await _expenseCategoryRepository.DeleteAsync(input.Id);
            }
           
        }

        public async Task<ListDto> GetExpenseCategoryForEdit(EntityDto input)
        {
            var items = (await _expenseCategoryRepository.GetAsync(input.Id)).MapTo<ListDto>();
            return items;

        }

        public bool ExpenseCategoryExsistenceById(EditDto input)
        {
            var items = _expenseCategoryRepository.GetAll().Where(e => e.Category == input.Category && e.Id != input.Id).Any();
            return items;
        }

        public async Task UpdateExpenseCategory(EditDto input)
        {
            var items = await _expenseCategoryRepository.GetAsync(input.Id);
            items.Category = input.Category;
            await _expenseCategoryRepository.UpdateAsync(items);
        }
        public async Task<ListDto> GetExpenseCategoryForDetail(EntityDto input)
        {
            var items = (await _expenseCategoryRepository.GetAsync(input.Id)).MapTo<ListDto>();
            return items;

        }

    }
}
