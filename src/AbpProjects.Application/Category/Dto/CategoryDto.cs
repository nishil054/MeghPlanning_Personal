using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category.Dto
{
    [AutoMapFrom(typeof(category))]
    public class CategoryDto : EntityDto
    {
        [Required]
        public virtual string Category { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual string[] TeamId { get; set; }
        public virtual List<string> Teams { get; set; }

    }
}
