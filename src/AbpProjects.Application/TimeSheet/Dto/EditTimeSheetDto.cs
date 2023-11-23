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
    [AutoMapTo(typeof(timesheet))]
    public class EditTimeSheetDto : EntityDto
    {
        [Required]
        public virtual int ProjectId { get; set; }
        [Required]
        public virtual int WorkTypeId { get; set; }
        [Required]


        public virtual string Description { get; set; }
        [Required]
        public virtual decimal Hours { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual int? UserStoryId { get; set; }
        public virtual decimal? Efforts { get; set; }
    }
}
