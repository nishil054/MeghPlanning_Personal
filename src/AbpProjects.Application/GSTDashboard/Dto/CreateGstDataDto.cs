using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.gstdashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.GSTDashboard.Dto
{
    [AutoMapTo(typeof(gstDashboard))]
    public class CreateGstDataDto: EntityDto
    {
        public virtual string Month { get; set; }
        public virtual decimal OutputGST { get; set; }
        public virtual decimal InputGST { get; set; }
        public virtual decimal TotalPayableGST { get; set; }
        public virtual decimal TotalPendingPayment { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual int FinancialyearId { get; set; }
        public virtual int Status { get; set; }
        public virtual int MonthId { get; set; }

        public virtual string MonthName { get; set; }
    }
}
