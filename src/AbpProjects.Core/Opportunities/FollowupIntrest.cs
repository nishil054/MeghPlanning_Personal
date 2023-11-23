using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Opportunities
{
    public class FollowupIntrest : FullAuditedEntity
    {
        public virtual int followupid { get; set; }
        public virtual int intestedid { get; set; }
    }
}
