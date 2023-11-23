using Abp.Domain.Entities.Auditing;
using AbpProjects.Opportunities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.CallCategories
{
    public class CallCategory: FullAuditedEntity
    {
        public string Name { get; set; }
       
    }
}
