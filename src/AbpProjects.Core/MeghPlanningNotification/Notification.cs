using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningNotification
{
    [Table("Notification")]
    public class Notification : FullAuditedEntity
    {
        public virtual string Title { get; set; }
    }
}
