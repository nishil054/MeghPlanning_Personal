using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Company.Dto
{
    [AutoMapTo(typeof(company))]
    public   class CreateCompanyDto : EntityDto
    {
        [Required]
        public virtual string Beneficial_Company_Name { get; set; }
    }
}
