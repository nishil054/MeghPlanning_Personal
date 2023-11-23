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
    [AutoMapTo(typeof(ExpenseCategory))]
    public class EditDto : EntityDto
    {
        public virtual string Category { get; set; }
    }
}
