using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Bills
{
    [Table("Bill")]
    public class Bill : AuditedEntity
    {
        
        [Required]
        [MaxLength(7)]
        public virtual string BillNo { get; set; }
        public virtual int? ClientID { get; set; }
        public virtual bool Cancel { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual DateTime? PrepDate { get; set; }


        [MaxLength(200)]
        public virtual string PrepBy { get; set; }
        [MaxLength(200)]
        public virtual string PartnerID { get; set; }

        [MaxLength(50)]
        public virtual string Comment { get; set; }
        public virtual int? SerTextID1 { get; set; }
        [MaxLength(1000)]
        public virtual string SText1 { get; set; }
        public virtual int? SerTextID2 { get; set; }
        [MaxLength(1000)]
        public virtual string SText2 { get; set; }

        public virtual int? SerTextID3 { get; set; }
        [MaxLength(1000)]
        public virtual string SText3 { get; set; }


        public virtual int? SerTextID4 { get; set; }
        [MaxLength(1000)]
        public virtual string SText4 { get; set; }
        public virtual int? SAmt1 { get; set; }
        public virtual int? SAmt2 { get; set; }
        public virtual int? SAmt3 { get; set; }
        public virtual int? SAmt4 { get; set; }

        public virtual int? STax { get; set; }
        public virtual int? SCess { get; set; }
        public virtual int? SHCess { get; set; }

        [MaxLength(100)]
        public virtual string BillTotal { get; set; }
        [MaxLength(10)]
        public virtual string Currency { get; set; }

        public virtual int? servicegroupid { get; set; }
        public virtual int? clientgroupid { get; set; }
        public virtual int? servicetypeid { get; set; }

        public virtual decimal? totalbillamount { get; set; }
        public virtual int? bankid { get; set; }
        public virtual int? servicetypeid2 { get; set; }
        public virtual int? servicetypeid3 { get; set; }
        public virtual int? servicetypeid4 { get; set; }

        public virtual decimal? swachhtax { get; set; }
        public virtual decimal? servicetax { get; set; }
        public virtual int? service_year { get; set; }
        public virtual decimal? servicetotal { get; set; }

        [MaxLength(200)]
        public virtual string address { get; set; }
        [MaxLength(100)]
        public virtual string citypin { get; set; }
        public virtual bool? isdefault { get; set; }

        [MaxLength(100)]
        public virtual string panno { get; set; }
        public virtual decimal? krishitax { get; set; }
        public virtual decimal? cgst { get; set; }
        public virtual decimal? sgst { get; set; }
        public virtual decimal? igst { get; set; }
        [MaxLength(200)]
        public virtual string userid { get; set; }

        public virtual int? statuscodeid { get; set; }
        [MaxLength(100)]
        public virtual string statuscodeno { get; set; }
        [MaxLength(100)]
        public virtual string gstin { get; set; }
        [MaxLength(200)]
        public virtual string sacno1 { get; set; }
        [MaxLength(200)]
        public virtual string sacno2 { get; set; }
        [MaxLength(200)]
        public virtual string sacno3 { get; set; }
        [MaxLength(200)]
        public virtual string sacno4 { get; set; }

        public virtual bool? isexport { get; set; }
        public virtual int? companyid { get; set; }
        [MaxLength(200)]
        public virtual string invoiceno { get; set; }
        [MaxLength(100)]
        public virtual string orderno { get; set; }

        public virtual DateTime? orderdate { get; set; }
        public virtual bool? isoutexport { get; set; }
        public virtual bool? isoutexport1 { get; set; }

        public virtual int? Performaid { get; set; }
        public virtual decimal? exchangerate { get; set; }


    }
}
