using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Company.Dto
{
    [AutoMapTo(typeof(company))]
public class EditCompanyDto : EntityDto
    {
        public string Beneficial_Company_Name { get; set; }
    }
}
