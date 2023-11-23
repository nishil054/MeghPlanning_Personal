using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    [AutoMapFrom(typeof(project))]
    public  class StatsAmountDto : EntityDto
    {
        public virtual string ProjectName { get; set; }
        public virtual decimal ProjectCost { get; set; }
        public virtual decimal CompanyCost { get; set; }
        public virtual decimal CostPercentage { get; set; }
        public virtual decimal Profit { get; set; }
        public virtual decimal ProfitPercentage { get; set; }
        public virtual int StatusId { get; set; }
        public virtual string Status { get; set; }
        public virtual int SortOrder { get; set; }
    }
}
