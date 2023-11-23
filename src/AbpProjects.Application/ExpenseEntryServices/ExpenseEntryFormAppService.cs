using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AbpProjects.ExpenseCategories;
using AbpProjects.ExpenseEnteryForm;
using AbpProjects.ExpenseEntryServices.Dto;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbpProjects.Reports.Dto;

namespace AbpProjects.ExpenseEntryServices
{
    class ExpenseEntryFormAppService : AbpProjectsApplicationModule, IExpenseEntryFormAppService
    {
        private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;
        private readonly IRepository<ExpenseSubcategory> _expenseSubCategoryRepository;
        private readonly IRepository<ExpenseEntry> _expenseEntryRepository;
        public ExpenseEntryFormAppService(IRepository<ExpenseCategory> expenseCategoryRepository, IRepository<ExpenseSubcategory> expenseSubCategoryRepository, IRepository<ExpenseEntry> expenseEntryRepository)
        {

            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseSubCategoryRepository = expenseSubCategoryRepository;
            _expenseEntryRepository = expenseEntryRepository;
        }
        public async Task CreateExpenseEntry(List<InsertDto> input, DateTime date)
        {
           
            foreach (var item in input)
            {
                
                var items = _expenseEntryRepository.GetAll().Where(e => e.Id == item.Id).FirstOrDefault();
                if(items==null)
                {
                    var entry = item.MapTo<ExpenseEntry>();
                    entry.MonthYear = date;
                    await _expenseEntryRepository.InsertAsync(entry);
                }
                else
                {
                    items.Expense = item.Expense;
                    items.MonthYear = date;
                    await _expenseEntryRepository.UpdateAsync(items);
                    
                }
            }
        }
        public ListResultDto<ListDto> GetExpenseEntry(DateTime monthyr)
        {
            try
            {

                var dt = (from h in _expenseCategoryRepository.GetAll()
                          join p in _expenseSubCategoryRepository.GetAll() on h.Id equals p.CategoryId
                          join f in _expenseEntryRepository.GetAll() on p.Id equals f.SubCategoryId into fg
                          from fgi in
                         fg.Where(f => f.MonthYear.Month == monthyr.Month && f.MonthYear.Year == monthyr.Year).DefaultIfEmpty()
                          select new ListDto
                          {
                              Id = fgi.Id == null ? 0 : fgi.Id,
                              Category = h.Category,
                              CategoryId = h.Id,
                              SubCategoryId = p.Id,
                              Expense = fgi.Expense == null ? 0 : fgi.Expense,
                              SubCategory = p.SubCategory
                              //SubCategory = subctaegoryrepo.Where(x => x.CategoryId == unit.e2.Id ).Select(c => c.SubCategory).ToList(),

                          }).Distinct().OrderBy(p => p.Category);

                var expentey = dt.ToList();


                 

                return new ListResultDto<ListDto>(expentey.MapTo<List<ListDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

      
    }
}
