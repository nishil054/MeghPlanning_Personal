using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Document.Dto
{
  public  class GetDocumentDto :PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Title { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Title";
            }
        }
    }
}
