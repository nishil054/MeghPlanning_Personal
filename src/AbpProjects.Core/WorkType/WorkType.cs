using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.WorkType
{
    [Table("WorkType")]
    public  class worktype : FullAuditedEntity
    {
        [Required]
        public virtual string WorkTypeName { get; set; }
    }
}
