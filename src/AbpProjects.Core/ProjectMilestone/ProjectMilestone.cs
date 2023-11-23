using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectMilestone
{
    [Table("TblProjectMilestone")]
    public class projectMilestone: FullAuditedEntity
    {
        public virtual string Title { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Description { get; set; }
        public virtual int ProjectTypeId { get; set; }
    }
}
