using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
    [AutoMapFrom(typeof(timesheet))]
    public class ReportsDto : EntityDto
    {
        public virtual int ProjectId { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal? Hours { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual int? NoOfProjects { get; set; }

    }

    public class ReportDetails
    {
        public virtual DateTime Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal? Hours { get; set; }
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
    }
}
