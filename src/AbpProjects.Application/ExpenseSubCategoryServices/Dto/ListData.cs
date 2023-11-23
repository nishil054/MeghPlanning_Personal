using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.ExpenseCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseSubCategoryServices.Dto
{
    public class GetExpenseSubcategoryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

    }
    [AutoMapFrom(typeof(ExpenseSubcategory))]
    public class ListData
    {
        public virtual int Id { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual string SubCategory { get; set; }
        public virtual string Category { get; set; }
    }
}
