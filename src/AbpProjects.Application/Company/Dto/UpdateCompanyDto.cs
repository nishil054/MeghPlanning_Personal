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
    [AutoMapFrom(typeof(company))]
 public  class UpdateCompanyDto : EntityDto
    {
        [Required]
        public string Beneficial_Company_Name { get; set; }
    }
}
