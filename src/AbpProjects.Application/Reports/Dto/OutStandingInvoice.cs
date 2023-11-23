using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Reports.Dto
{
   
    public class OutStandingInvoice : PagedAndSortedResultRequestDto
    {
        public string InvoiceNo { get; set; }
        public string ClientName { get; set; }
        public bool? IsMarkAsConfirm { get; set; }
        public decimal? TotalBillAmt { get; set; }
        public decimal? TotalCollection { get; set; }
        public decimal? OutStandingAmt { get; set; }
        public int ClientId { get; set; }
        public int? PerformaId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public  DateTime? BillDate { get; set; }
    }
    public class ImportOutStandingInvoiceDto: PagedAndSortedResultRequestDto
    {
        public int? ClientId { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
