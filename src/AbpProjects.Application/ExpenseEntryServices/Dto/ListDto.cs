using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.ExpenseEnteryForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseEntryServices.Dto
{
    public class GetExpenseEntryInput /*: PagedAndSortedResultRequestDto*/
    {
        //public string Filter { get; set; }
        public virtual DateTime MonthYear { get; set; }
       
    }
    [AutoMapFrom(typeof(ExpenseEntry))]
    public  class ListDto
    {
        public virtual int Id { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual int SubCategoryId { get; set; }
        public virtual DateTime MonthYear { get; set; }
        public virtual decimal Expense { get; set; }

        public virtual string Category { get; set; }
        public virtual string SubCategory { get; set; }
    }

    public class resultListDto
    {
        public virtual int Id { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual int SubCategoryno { get; set; }
        public virtual DateTime MonthYear { get; set; }
        public virtual decimal Expense { get; set; }

        public virtual string Category { get; set; }
        public virtual string SubCategory { get; set; }
    }
}
