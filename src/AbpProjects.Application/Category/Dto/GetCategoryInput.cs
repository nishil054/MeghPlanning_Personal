using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category.Dto
{
    public class GetCategoryInput : PagedAndSortedResultRequestDto
    {
        public string CategoryFilter { get; set; }
        public int? TeamFilter { get; set; }
    }
}
