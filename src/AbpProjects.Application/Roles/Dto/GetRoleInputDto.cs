using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Roles.Dto
{
    public class GetRoleInputDto : PagedAndSortedResultRequestDto
    {
        public virtual string DisplayName { get; set; }
    }
}
