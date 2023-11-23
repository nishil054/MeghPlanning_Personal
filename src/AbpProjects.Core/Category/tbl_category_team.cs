using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Category
{
    [Table("Category_Team")]
  public class tbl_category_team : FullAuditedEntity
    {
        public virtual int CategoryId { get; set; }
        public virtual int TeamId { get; set; }
    }
}

