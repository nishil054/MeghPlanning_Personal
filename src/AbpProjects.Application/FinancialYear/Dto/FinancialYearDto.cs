using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FinancialYear.Dto
{
    public class FinancialYearDto: EntityDto
    {
        public string Year { get; set; }
    }
}
