using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category.Dto
{
    [AutoMapTo(typeof(category))]
    public class CreateCategoryDto
    {
        [Required]
        public virtual string Category { get; set; }
        public virtual string[] TeamId { get; set; }
    }
}
