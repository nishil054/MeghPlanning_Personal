using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapFrom(typeof(project))]
    public class ProjectDto : EntityDto
    {
        public const int maxLengthProjectName = 250;
        public const int maxLengthCompanyName = 250;
        public const int maxLengthDescription = 500;
        [Required]
        public virtual int BeneficiaryCompanyId { get; set; }
        [Required]
        public virtual string ProjectName { get; set; }
        [Required]
        public virtual string Description { get; set; }
        [Required]
        public virtual DateTime StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }
       
        public virtual DateTime? TeamDeadline { get; set; }
        public virtual DateTime? ActualEndDate { get; set; }
        [Required]
        public virtual string CompanyName { get; set; }
        public virtual int? Marketing_LeaderId { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string MarketingLeadName { get; set; }
        public virtual string ProjectStatus { get; set; }
        public virtual int ProjectStatusId { get; set; }
        public virtual decimal hourPercentage { get; set; }

        public virtual decimal totalhours { get; set; }
        public virtual decimal actualhours { get; set; }
        public virtual decimal hoursum { get; set; }
        public List<StatusddlList> objProjectStatusList { get; set; }
        public virtual decimal? pricesum { get; set; }
        public virtual decimal? PendingAmount { get; set; }
        public virtual decimal? Invoiceamount { get; set; }
        public virtual decimal sixtyper { get; set; }
        public virtual decimal eightyper { get; set; }
        public virtual string username { get; set; }
        public virtual int uid { get; set; }
        public virtual decimal totalhourssum { get; set; }
        public virtual decimal ProjectCost { get; set; }
        public virtual decimal profit { get; set; }
        public virtual decimal profitper { get; set; }
        public virtual string statusname { get; set; }
        public virtual int? Priority { get; set; }

        public virtual int enableStatus { get; set; }
        public virtual bool? typeshow { get; set; }

    }
    public class StatusddlList
    {
        public virtual int statusId { get; set; }
        public virtual string statusname { get; set; }
    }
}
