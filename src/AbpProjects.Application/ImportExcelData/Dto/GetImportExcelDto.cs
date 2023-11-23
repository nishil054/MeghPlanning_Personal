using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.ImportExcelData.Dto
{
    public class GetImportExcelDto : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }
    }
}
