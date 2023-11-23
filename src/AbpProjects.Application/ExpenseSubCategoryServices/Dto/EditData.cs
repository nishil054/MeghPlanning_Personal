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
    [AutoMapTo(typeof(ExpenseSubcategory))]
    public class EditData : EntityDto
    {
        public virtual int Id { get; set; }
        public virtual string SubCategory { get; set; }
        public virtual int CategoryId { get; set; }
    }
}
