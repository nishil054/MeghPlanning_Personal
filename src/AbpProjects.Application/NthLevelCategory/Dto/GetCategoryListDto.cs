using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NthLevelCategory.Dto
{
    [AutoMapFrom(typeof(CategoryMaster))]
    public class GetCategoryListDto : EntityDto
    {
        public virtual string Name { get; set; }
        public virtual int SortOrder { get; set; }
    }
    public class GetMasterInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int CategoryId { get; set; }
    }
}
