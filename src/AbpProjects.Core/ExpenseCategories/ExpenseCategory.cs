using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseCategories
{
   public  class ExpenseCategory : FullAuditedEntity
    {
        public const int maxCategoryLength = 500;
        [Required]
        [MaxLength(maxCategoryLength)]
        public virtual string Category { get; set; }
    }
}
