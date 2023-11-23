using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.BulkData
{
    [Table("BulkMaster")]
    public class bulkmaster : FullAuditedEntity
    {
        public virtual int EmpId { get; set; }
        public virtual string EmpName { get; set; }
        public virtual string EmpEmail { get; set; }
        public virtual int Contact { get; set; }
    }
}
