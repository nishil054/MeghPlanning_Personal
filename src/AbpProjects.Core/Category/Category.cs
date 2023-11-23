using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category
{
    [Table("Category")]
    public class category : FullAuditedEntity
    {
        public const int maxCategoryLength = 500;
        [Required]
        [MaxLength(maxCategoryLength)]
        public virtual string Category { get; set; }
    }
}
