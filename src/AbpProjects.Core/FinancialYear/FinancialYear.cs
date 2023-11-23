using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FinancialYear
{
    [Table("FinancialYear")]
    public class financialYear: FullAuditedEntity
    {
        public virtual int StartYear { get; set; }
        public virtual int EndYear { get; set; }
        public virtual string Title { get; set; }
    }
}
