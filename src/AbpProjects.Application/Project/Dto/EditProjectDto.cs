using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    [AutoMapFrom(typeof(project))]
    public  class EditProjectDto : EntityDto
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
        public virtual string CompanyName { get; set; }

        public virtual int? Marketing_LeaderId { get; set; }

        public virtual decimal Price { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string totalhours { get; set; }
    }
}
