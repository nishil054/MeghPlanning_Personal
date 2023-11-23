using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.Team;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Users.Dto
{
    [AutoMapFrom(typeof(team))]
 public  class TeamDto : EntityDto
    {
        [Required]
        public virtual string TeamName { get; set; }
    }
}
