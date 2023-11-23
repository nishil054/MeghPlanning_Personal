using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.OpportunityAppServices.Dto
{
    [AutoMapFrom(typeof(Followuptype))]
    public class FollowUpTypeDto : EntityDto
    {
        [Required]
        public virtual string FollowUpType { get; set; }
    }
}
