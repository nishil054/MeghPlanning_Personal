using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Project.Dto
{
    public class GetProjectDto :PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public int? ClientId { get; set; }
        public int? MarketingleadId { get; set; }
        public int[] ProjectStatusId { get; set; }
        
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "ProjectName";
            }
        }
    }
    public class ImportGetProjectDto
    {
        public string SearchBy { get; set; }
        public int? ClientId { get; set; }
        public int? MarketingleadId { get; set; }
        public int?[] ProjectStatusId { get; set; }
    }
}
