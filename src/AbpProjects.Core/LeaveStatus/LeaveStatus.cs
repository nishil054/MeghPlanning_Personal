using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveStatus
{
    [Table("LeaveStatus")]
   public class Leavestatus : FullAuditedEntity
    {
        public virtual string Status { get; set; }
    }
}
