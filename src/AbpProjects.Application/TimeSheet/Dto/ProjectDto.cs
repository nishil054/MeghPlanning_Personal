using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet.Dto
{
    [AutoMapFrom(typeof(project))]
    public class ProjectDto : EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual string Marketing_LeaderName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string ProjectType { get; set; }
        public virtual int ptid { get; set; }
        public virtual DateTime ProjectCreatedate { get; set; }
        public virtual string Description { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }
        public virtual int? Marketing_LeaderId { get; set; }

        public virtual int? TimeSheetCount { get; set; }
        public virtual DateTime ProjectTypedate { get; set; }
    }

    public class GetProjectInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int? ProjectId { get; set; }
        public int? ClientId { get; set; }
        public int? MarketingleadId { get; set; }
        public int? ProjectStatusId { get; set; }
        public virtual decimal? eightyper { get; set; }
        public virtual decimal? actualhours { get; set; }
        public virtual decimal? hourPercentage { get; set; }
    }
}
