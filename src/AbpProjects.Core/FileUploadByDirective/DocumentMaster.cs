using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FileUploadByDirective
{
    [Table("DocumentMaster")]
   public class Documentmaster : FullAuditedEntity
    {
        public virtual string DocumentComment { get; set; }
    }

    
}
