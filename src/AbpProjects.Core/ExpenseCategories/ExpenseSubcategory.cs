using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ExpenseCategories
{
    public class ExpenseSubcategory : FullAuditedEntity
    {
        public const int maxSubCategoryLength = 500;
        [Required]
        [MaxLength(maxSubCategoryLength)]

        public virtual string SubCategory { get; set; }
        public virtual int CategoryId { get; set; }
    }
}
