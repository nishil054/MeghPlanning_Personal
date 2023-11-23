using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.GSTDashboard.Dto
{
    public class GetMonthNameDto: EntityDto
    {
        public string MonthName { get; set; }
    }
}
