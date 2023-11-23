using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices.Dto
{
    [AutoMapFrom(typeof(Opportunity))]
    public class ProjectTypeDto:EntityDto
    {
        public virtual int CreatorUserId { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual ProjectList[] ProjectType_Name { get; set; }
    }

    public class ProjectList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
