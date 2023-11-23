using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Productions
{
    public class Production : AuditedEntity
    {

        public virtual int Invoiceid { get; set; }
        public virtual int Invoicetype { get; set; }
        public virtual DateTime Invoicedate { get; set; }
        public virtual decimal InvoiceAmount { get; set; }


        public int Serviceid { get; set; }

        public int Projectid { get; set; }

        public int Requestid { get; set; }

        public int ProductionFlag { get; set; }

        public virtual decimal TotalInvoiceAmount { get; set; }

    }
}
