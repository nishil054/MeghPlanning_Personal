using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AbpProjects.ManageKnowledgeCenter.Dto
{
    [AutoMapTo(typeof(manageKnowledgeCenter))]
    public class CreateKnowledgeCenterDto
    {
        public virtual int TeamId { get; set; }
        public virtual string Title { get; set; }
        public virtual int IsDocument { get; set; }
        public virtual string Document { get; set; }
        public virtual string Url { get; set; }
        public virtual string Comment { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual List<string> Attachments { get; set; }
        public virtual HttpPostedFileBase Attachmentpath { get; set; }
        public virtual List<string> NewAttachments { get; set; }
    }

    public class ProjectAttachmentView
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string NewFileName { get; set; }
    }

    public class KnowledgeDocumentsView : EntityDto
    {
        public int KnowledgeCenterId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string DocumentName { get; set; }
    }
}
