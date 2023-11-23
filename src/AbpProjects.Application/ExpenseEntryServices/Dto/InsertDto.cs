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
    [AutoMapTo(typeof(ExpenseEntry))]
    
    public class InsertDto : EntityDto
    {
        //public virtual int Id { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual int SubCategoryId { get; set; }
        public virtual DateTime MonthYear { get; set; }
        public virtual decimal Expense { get; set; }

        public virtual string Category { get; set; }
        public virtual string SubCategory { get; set; }

    }
}
