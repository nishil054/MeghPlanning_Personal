using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter
{
    [Table("ManageKnowledgeCenter")]
    public class manageKnowledgeCenter : FullAuditedEntity
    {
        public virtual int TeamId { get; set; }
        public virtual string Title { get; set; }
        public virtual int IsDocument { get; set; }
        //public virtual string Document { get; set; }
        public virtual string Url { get; set; }
        public virtual string Comment { get; set; }
        public virtual int CategoryId { get; set; }
    }
}
