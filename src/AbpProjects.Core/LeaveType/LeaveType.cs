using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.LeaveType
{
    [Table("LeaveType")]
   public class Leavetype : FullAuditedEntity
    {
        public virtual string Type { get; set; }
    }
}
