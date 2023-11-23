using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document.Dto
{
    [AutoMapTo(typeof(document))]
    public class CreateDocumentDto : EntityDto
    {
        public virtual string Title { get; set; }
        
        public virtual string Attachment { get; set; }
    }

    public class DocumentAttachmentView
    {
        public virtual List<string> FileName { get; set; }
        public virtual List<string> FilePath { get; set; }
    }
}
