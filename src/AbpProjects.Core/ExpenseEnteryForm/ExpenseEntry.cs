using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseEnteryForm
{
    public class ExpenseEntry : FullAuditedEntity
    {
        public virtual int CategoryId { get; set; }
        public virtual int SubCategoryId { get; set; }
        public virtual DateTime MonthYear { get; set; }
        public virtual decimal Expense { get; set; }

    }
}
