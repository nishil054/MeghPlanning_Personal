using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AbpProjects.FileUploadByDirective;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document.Dto
{
    [AutoMapTo(typeof(Documentmaster))]
   public class CreateFileUploadDto : EntityDto
    {
        public virtual string DocumentComment { get; set; }
    }
}
