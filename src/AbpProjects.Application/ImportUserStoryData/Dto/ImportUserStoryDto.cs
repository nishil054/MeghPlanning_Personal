using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportUserStoryData.Dto
{
    [AutoMapFrom(typeof(ImportUserStoryDetails))]
    public class ImportUserStoryDto: EntityDto
    {
        public virtual string UserStory { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual int status { get; set; }
        public virtual decimal DeveloperHours { get; set; }
        public virtual decimal ExpectedHours { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual decimal ActualHours { get; set; }
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual int Userstorycount { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual DateTime AssignToDate { get; set; }
        public virtual DateTime CreationDate { get; set; }
    }

    public class GetImportUserstoryDto : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public virtual int? status { get; set; }
        public virtual int? Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public virtual string UserStory { get; set; }
    }
}
