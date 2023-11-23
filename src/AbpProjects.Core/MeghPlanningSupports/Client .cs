using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningSupports
{
    [Table("Clients")]
    public class Client : AuditedEntity
    {
        public virtual int Id { get; set; }
        public virtual string ClientName { get; set; }
        public virtual int GroupID { get; set; }
        public virtual string ClientAddr1 { get; set; }
        public virtual string ClientAddr2 { get; set; }
        public virtual string ClientCity { get; set; }
        public virtual string ClientState { get; set; }
        public virtual string ClientPIN { get; set; }
        public virtual string ClientContact { get; set; }
        public virtual string ClientEmail { get; set; }
        public virtual bool UnderService { get; set; }
        public virtual string TANNO { get; set; }
        public virtual string PANNO { get; set; }
        public virtual int sectorid { get; set; }
        public virtual int companyid { get; set; }
        public virtual string clientcode { get; set; }
        public virtual string pan_no { get; set; }
        public virtual int? Clientid { get; set; }

    }
}
