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
    [AutoMapFrom(typeof(gstDashboard))]
    public class GetGstDataListDto: EntityDto
    {
        public virtual string Month { get; set; }
        public virtual string MonthName { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual decimal OutputGST { get; set; }
        public virtual decimal? InputGST { get; set; }
        public virtual decimal TotalPayableGST { get; set; }
        public virtual decimal TotalPendingPayment { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual int FinancialyearId { get; set; }
        public virtual int? Status { get; set; }
        public virtual int? MonthId { get; set; }
        public virtual int? Mid { get; set; }
        
        public virtual int? MonthFromDrop { get; set; }
        public virtual decimal? Cgst { get; set; }
        public virtual decimal? Sgst { get; set; }
        public virtual decimal? Igst { get; set; }

        public virtual decimal? totalgst { get; set; }
        public virtual decimal? payablegst { get; set; }
        public virtual bool InvoiceDisable { get; set; }
        public virtual int EndYear { get; set; }

    }
}
