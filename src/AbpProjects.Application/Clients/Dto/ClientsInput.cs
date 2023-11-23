using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Clients.Dto
{
    public class ClientsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string search { get; set; }
        public virtual string Name { get; set; }
        public void Normalize()
        {
            if (String.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}