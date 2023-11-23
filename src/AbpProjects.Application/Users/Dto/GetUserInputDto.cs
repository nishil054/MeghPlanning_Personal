using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace AbpProjects.Users.Dto
{
    public class GetUserInputDto : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Name { get; set; }
        public int? TeamId { get; set; }
        public int? RoleId { get; set; }
        public decimal LeaveBalance { get; set; }
        public int? CompanyId { get; set; }
        public int? ActiveStatus { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }
    }
}
