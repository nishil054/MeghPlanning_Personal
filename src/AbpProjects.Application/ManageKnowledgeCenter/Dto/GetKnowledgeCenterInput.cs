using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter.Dto
{
    public class GetKnowledgeCenterInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public virtual int? TeamId { get; set; }
        public virtual int? CategoryId { get; set; }

    }
}
