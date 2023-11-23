using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.NthLevelCategory
{
    public class CategoryMaster : FullAuditedEntity
    {
        public virtual string Name { get; set; }
        public virtual int LeftExtent { get; set; }
        public virtual int RightExtent { get; set; }
        public virtual int SortOrder { get; set; }
    }
}
