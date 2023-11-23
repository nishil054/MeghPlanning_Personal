using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AbpProjects.ExpenseCategories;
using AbpProjects.ExpenseSubCategoryServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace AbpProjects.ExpenseSubCategoryServices
{
    public class ExpSubCategoryAppService : AbpProjectsApplicationModule, IExpSubCategoryAppService
    {
        private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;
        private readonly IRepository<ExpenseSubcategory> _expenseSubCategoryRepository;
        public ExpSubCategoryAppService(IRepository<ExpenseCategory> expenseCategoryRepository, IRepository<ExpenseSubcategory> expenseSubCategoryRepository)
        {

            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseSubCategoryRepository = expenseSubCategoryRepository;
        }
        public bool ExpenseSubCategoryExsistence(InsertData input)
        {

            var items = _expenseSubCategoryRepository.GetAll().Where(e => e.SubCategory == input.SubCategory && e.CategoryId==input.CategoryId).Any();
            return items;
        }

        public async Task ExpenseCreateSubCategory(InsertData input)
        {
            var category = input.MapTo<ExpenseSubcategory>();
            await _expenseSubCategoryRepository.InsertAsync(category);
        }

        public PagedResultDto<ListData> GetExpenseSubCategory(GetExpenseSubcategoryInput Input)
        {
            try
            {
                var cate = (from e1 in _expenseSubCategoryRepository.GetAll()
                            join e2 in _expenseCategoryRepository.GetAll() on e1.CategoryId equals e2.Id

                            select new ListData
                            {
                                Id = e1.Id,
                                CategoryId = e1.CategoryId,
                                Category = e2.Category,
                                SubCategory = e1.SubCategory,

                            })
                            .WhereIf(!Input.SubCategory.IsNullOrEmpty() && Input.SubCategory != "",
                            p => p.SubCategory.ToLower().Contains(Input.SubCategory.ToLower()))
                  .WhereIf(!Input.Category.IsNullOrEmpty() && Input.Category != "",
                            p => p.Category.ToLower().Contains(Input.Category.ToLower()));


                //.OrderByDescending(p => p.Id).ToList();

                var totalcount = cate.Count();
                //var categoryPageBy = cate.AsQueryable().PageBy(Input);
                var categoryPageBy = cate.OrderBy(Input.Sorting).PageBy(Input).ToList();

                return new PagedResultDto<ListData>(totalcount, categoryPageBy.MapTo<List<ListData>>());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task DeleteExpenseSubCategory(EntityDto input)
        {
            await _expenseSubCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<ListData> GetExpenseSubCategoryForEdit(EntityDto input)
        {
            var items = (await _expenseSubCategoryRepository.GetAsync(input.Id)).MapTo<ListData>();
            return items;

        }

        public bool ExpenseSubCategoryExsistenceById(EditData input)
        {
            var items = _expenseSubCategoryRepository.GetAll().Where(e => e.SubCategory == input.SubCategory && e.Id != input.Id && e.CategoryId == input.CategoryId).Any();
            return items;
        }

        public async Task UpdateExpenseSubCategory(EditData input)
        {
            var items = await _expenseSubCategoryRepository.GetAsync(input.Id);
            items.CategoryId = input.CategoryId;
            items.SubCategory = input.SubCategory;
            await _expenseSubCategoryRepository.UpdateAsync(items);
        }
        public async Task<ListData> GetExpenseSubCategoryForDetail(EntityDto input)
        {
            var items = (await _expenseSubCategoryRepository.GetAsync(input.Id)).MapTo<ListData>();
            return items;

        }

        public ListResultDto<ListData> GetCategory(GetExpenseSubcategoryInput input)
        {
            var persons = (from item in _expenseCategoryRepository
                 .GetAll()
                           select new ListData
                           {
                               CategoryId = item.Id,
                               Category = item.Category
                           })
                           .OrderBy(x => x.Category).ToList();

            return new ListResultDto<ListData>(persons.MapTo<List<ListData>>());


        }

        public async Task<ListData> GetExpenseCategoryForDetail(EntityDto input)
        {
           
            var items = (from e1 in _expenseSubCategoryRepository.GetAll()
                                    join e2 in _expenseCategoryRepository.GetAll() on e1.CategoryId equals e2.Id
                                    where e1.Id==input.Id
                                    select new ListData
                                    {
                                        Id = e1.Id,
                                        CategoryId = e1.CategoryId,
                                        Category = e2.Category,
                                        SubCategory = e1.SubCategory,



                                    }).FirstOrDefault();

                   
            return items;

        }

    }
}
