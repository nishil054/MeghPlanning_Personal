using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace AbpProjects.Receipt
{

    [Table("BillPymtRecd")]
    public class BillPymtRecd : AuditedEntity
    {
        public virtual int RcptID { get; set; }
        [MaxLength(7)]
        public virtual string BillNo { get; set; }
        public virtual bool FullPayment { get; set; }
        public virtual DateTime? RcptDate { get; set; }
        public virtual int? PymtRecd { get; set; }


        [MaxLength(6)]
        public virtual string Mode { get; set; }
        [MaxLength(10)]
        public virtual string ChequeNo { get; set; }

        [MaxLength(30)]
        public virtual string Bank { get; set; }
        [MaxLength(20)]
        public virtual string ChequeDate { get; set; }
        public virtual int? TDS { get; set; }
        public virtual int? Writeoff { get; set; }
        public virtual int? RSTax { get; set; }
        public virtual int? RCess { get; set; }
        public virtual int? RHCess { get; set; }
        public virtual int? bankid { get; set; }

        public virtual decimal? swachhtax { get; set; }
        public virtual int? paymentamount { get; set; }
        public virtual decimal? servicetax { get; set; }
        public virtual decimal? krishitax { get; set; }
        public virtual decimal? igst { get; set; }
        public virtual decimal? sgst { get; set; }
        public virtual decimal? cgst { get; set; }
        public virtual string userid { get; set; }
        public virtual int? Billid { get; set; }
    }
}
