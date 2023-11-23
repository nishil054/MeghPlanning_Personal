using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.ExpenseCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseCategoryService.Dto
{
    public class GetExpensecategoryInput : PagedAndSortedResultRequestDto
    {
        public string CategoryFilter { get; set; }

    }
    [AutoMapFrom(typeof(ExpenseCategory))]
    public class ListDto
    {
        public virtual int Id { get; set; }
        public virtual string Category { get; set; }
    }
}
