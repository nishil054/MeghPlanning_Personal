using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Opportunities
{
    public class OpportunityFollowUp : FullAuditedEntity
    {
        public virtual int opporutnityid { get; set; }
        public virtual DateTime? nextactiondate { get; set; }
        public virtual DateTime? expectedclosingdate { get; set; }
        public virtual string Comment { get; set; }
        public virtual int CalllCategoryId { get; set; }
        public virtual int Followuptypeid { get; set; }
    }
}
