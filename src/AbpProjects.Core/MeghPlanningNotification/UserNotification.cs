using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.MeghPlanningNotification
{
    [Table("UserNotification")]
   public class UserNotification : FullAuditedEntity
    {
        public virtual int NotificationId { get; set; }
        public virtual long UserId { get; set; }
    }
}
