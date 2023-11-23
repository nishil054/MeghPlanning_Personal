using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter
{
    [Table("KnowledgeDocuments")]
    public class KnowledgeDocuments : FullAuditedEntity
    {
        public int KnowledgeCenterId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string DocumentName { get; set; }
    }
}
