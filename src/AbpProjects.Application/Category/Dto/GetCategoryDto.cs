using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category.Dto
{
    [AutoMapFrom(typeof(category))]
    public class GetCategoryDto : EntityDto
    {
        public virtual string Category { get; set; }
        public int CategoryId { get; set; }
    }
}
