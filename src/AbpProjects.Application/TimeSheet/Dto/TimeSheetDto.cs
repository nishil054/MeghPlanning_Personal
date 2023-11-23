using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TimeSheet.Dto
{
    [AutoMapFrom(typeof(timesheet))]
    public class TimeSheetDto : EntityDto
    {

        public virtual int ProjectId { get; set; }
        public virtual string ProjectName { get; set; }


        public virtual int WorkTypeId { get; set; }
        public virtual string WorkTypeName { get; set; }


        public virtual string Description { get; set; }

        public virtual decimal Hours { get; set; }

        public virtual DateTime? Date { get; set; }
        public virtual int UserId { get; set; } 
        public virtual string UserName { get; set; }
        public virtual int? UserStoryId { get; set; }
        public virtual string UserStory { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual decimal? Efforts { get; set; }
        //public virtual string UserName { get; set; }
        public virtual bool IsEdit { get; set; }
        public virtual decimal ProjectPrice { get; set; }
        public virtual List<decimal> totalHours { get; set; }
        public virtual int totalcount { get; set; }
        //public virtual int? t { get; set; }

    }
    public class inputmaster : PagedAndSortedResultRequestDto
    {
        public virtual int? ProjectId { get; set; }
        public virtual int? UserId { get; set; }
        //public virtual int? t { get; set; }


    }

    [AutoMapFrom(typeof(timesheet))]
    public class TimeSheetData : EntityDto
    {
        public virtual int? ProjectId { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual int? WorkTypeId { get; set; }
        public virtual string WorkTypeName { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal? Hours { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual int? TimesheetComplete { get; set; }
        public virtual int TimesheetCount { get; set; }
    }
}
