using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FinancialYear.Dto
{
    [AutoMapTo(typeof(financialYear))]
    public class CreateFinancialyearDto: EntityDto
    {
        public virtual int StartYear { get; set; }
        public virtual int EndYear { get; set; }
        public virtual string Title { get; set; }  
        
    }
}
