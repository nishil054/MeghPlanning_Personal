using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project
{
    public class ProjectStatus : FullAuditedEntity
    {
        public virtual string Status { get; set; }
        public virtual int sortorder { get; set; }
    }
}
