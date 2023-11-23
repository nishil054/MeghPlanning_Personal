using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter.Dto
{
    [AutoMapFrom(typeof(manageKnowledgeCenter))]
    public class KnowledgeCenterDto : EntityDto
    {
        public virtual string Title { get; set; }
        public int KnowldgeCenterId { get; set; }
    }
}
