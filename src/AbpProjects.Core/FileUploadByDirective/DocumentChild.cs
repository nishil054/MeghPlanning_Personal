using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.FileUploadByDirective
{
    [Table("DocumentChild")]
    public class Documentchild : FullAuditedEntity
    {
        public virtual int DocumentId { get; set; }
        public virtual string FileName { get; set; }
    }
  
}
