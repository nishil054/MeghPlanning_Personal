using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NthLevelCategory.Dto
{
    [AutoMapTo(typeof(CategoryMaster))]
    public class CategoryCreateDto
    {
        public virtual string Name { get; set; }
        public virtual int sortOrder { get; set; }
    }
}
