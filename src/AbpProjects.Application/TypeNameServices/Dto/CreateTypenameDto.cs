using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.MeghPlanningSupports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.TypenameServices.Dto
{
    [AutoMapTo(typeof(Typename))]
  public class CreateTypenameDto : EntityDto
    {
        [Required]
        public virtual string Name { get; set; }
    }
}
