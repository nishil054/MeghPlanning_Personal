using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Company
{
    [Table("Company")]
  public  class company : FullAuditedEntity
    {
        public const int maxLength = 200;
        [Required]
        [MaxLength(maxLength)]
        public virtual string Beneficial_Company_Name { get; set; }
        public virtual int CompanyId { get; set; }
    }
}
