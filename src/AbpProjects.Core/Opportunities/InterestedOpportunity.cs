using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Opportunities
{
    public class InterestedOpportunity: FullAuditedEntity
    {
        public int Opportunityid { get; set; }
        public int projectypeid { get; set; }
    }
}
