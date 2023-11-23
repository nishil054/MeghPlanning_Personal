using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FinancialYear.Dto
{
    [AutoMapFrom(typeof(financialYear))]
    public class GetFinancialYearDto: EntityDto
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Title { get; set; }
    }
}
