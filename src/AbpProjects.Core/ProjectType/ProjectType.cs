using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ProjectType
{
    [Table("ProjectType")]
    public  class projecttype : FullAuditedEntity
    {
        public const int ProjectTypeLength = 100;
        [Required]
        public virtual string ProjectTypeName { get; set; }
    }
}
