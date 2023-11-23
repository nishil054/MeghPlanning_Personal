using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Company.Dto
{
    public class GetCompanyDto : PagedAndSortedResultRequestDto
    {
        public string Beneficial_Company_Name { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Beneficial_Company_Name";
            }
        }
    }
}
