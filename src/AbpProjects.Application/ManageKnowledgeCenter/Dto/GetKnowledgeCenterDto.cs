using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ManageKnowledgeCenter.Dto
{
    [AutoMapFrom(typeof(manageKnowledgeCenter))]
    public class GetKnowledgeCenterDto : EntityDto
    {
        public virtual string Title { get; set; }
        public int KnowldgeCenterId { get; set; }
        public virtual int TeamId { get; set; }
        public virtual int IsDocument { get; set; }
        //public virtual string Document { get; set; }
        public virtual string Url { get; set; }
        public virtual string Comment { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual string Team { get; set; }
        public virtual string Category { get; set; }
        public virtual List<KnowledgeDocumentsView> KnowledgeDocuments { get; set; }
    }
}
