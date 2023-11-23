using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter.Dto
{
    [AutoMapFrom(typeof(team))]
    public class GetTeamListDto : EntityDto
    {
        public virtual string TeamName { get; set; }
    }
}
