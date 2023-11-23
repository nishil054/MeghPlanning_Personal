using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AbpProjects.Holidays.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.Holidays
{
    public interface IHolidayAppService: IApplicationService
    {
        Task<PagedResultDto<HolidayListInputDto>> GetHolidayList(HoildayMasterInput input);
        Task<PagedResultDto<HolidayListInputDto>> GetHolidayListing(HoildayMasterInput input);
        Task<CreateHolidayDto> CreateHoliday(CreateHolidayDto input);
        Task<HolidayListInputDto> GetHolidayEdit(EntityDto input);
        Task UpdateHoliday(EditHoildayDto input);
        Task DeleteHoliday(EntityDto inputs);
    }
}
