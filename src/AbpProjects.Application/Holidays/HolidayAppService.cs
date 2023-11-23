using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using AbpProjects.Holidays.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.UI;
using Abp.Authorization;
using AbpProjects.Authorization;
using Abp.Runtime.Session;
using AbpProjects.Authorization.Users;
using Abp.Authorization.Users;
using AbpProjects.Authorization.Roles;
using System.Linq.Dynamic.Core;

namespace AbpProjects.Holidays
{
    //[AbpAuthorize(PermissionNames.Pages_DataVault_Admin_Holiday)]
    [AbpAuthorize(PermissionNames.Pages_DataVault_Admin_Holiday, PermissionNames.Pages_Employee_Holiday)]
    public class HolidayAppService : AbpProjectsApplicationModule, IHolidayAppService
    {
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IAbpSession _session;

        public HolidayAppService(IRepository<Holiday> holidayRepository, IRepository<User, long> userRepository, IRepository<Role> roleRepository, IRepository<UserRole, long> userRoleRepository, IAbpSession session)
        {
            _holidayRepository = holidayRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;

        }

        public async Task<CreateHolidayDto> CreateHoliday(CreateHolidayDto input)
        {
            
            CreateHolidayDto obj = new CreateHolidayDto();
            
            
                try
                {
                    var holidaycreate = input.MapTo<Holiday>();//ObjectMapper.Map<WorkType>(input);
                    int Id = await _holidayRepository.InsertAndGetIdAsync(holidaycreate);

                    var data = _holidayRepository.Get(Id);
                    obj.StartDate = data.StartDate;
                    obj.EndDate = data.EndDate;
                    obj.Type = data.Type;
                    obj.Title = data.Title;
                    return obj;

                }
                catch (Exception ex) { }
            
            return obj = null;
        }

        public async Task DeleteHoliday(EntityDto inputs)
        {
            await _holidayRepository.DeleteAsync(inputs.Id);
        }

        public async Task<HolidayListInputDto> GetHolidayEdit(EntityDto input)
        {
            var sname = (await _holidayRepository.GetAsync(input.Id)).MapTo<HolidayListInputDto>();
            return sname;
        }

        public async Task<PagedResultDto<HolidayListInputDto>> GetHolidayList(HoildayMasterInput input)
        {
            int curId = (int)_session.UserId;
            string roleName = "";

            var roleId = _userRoleRepository.GetAll().Where(x => x.UserId == curId).Select(x => x.RoleId).FirstOrDefault();
            if (roleId != null)
            {
                roleName = _roleRepository.GetAll().Where(x => x.Id == roleId).Select(x => x.Name).FirstOrDefault();
            }

           
                var year = DateTime.Now.Year.ToString();
                DateTime currentdate = DateTime.Now;
                var timeCurrent = currentdate.ToString("yyyy-MM-dd");
                DateTime dateCurrent = DateTime.Parse(timeCurrent);
                var holidays = _holidayRepository.GetAll()
                                                 .WhereIf(!input.Title.IsNullOrEmpty(), p => p.Title.ToLower().Contains(input.Title.ToLower()));

                                           
                var ccData = holidays.OrderBy(input.Sorting).PageBy(input).ToList();
                var ccCount = holidays.Count();

                
                return new PagedResultDto<HolidayListInputDto>(ccCount, ccData.MapTo<List<HolidayListInputDto>>());
            //}
        }

        public async Task<PagedResultDto<HolidayListInputDto>> GetHolidayListing(HoildayMasterInput input)
        {
            var year = DateTime.Now.Year.ToString();
            DateTime currentdate = DateTime.Now;
            var timeCurrent = currentdate.ToString("yyyy-MM-dd");
            DateTime dateCurrent = DateTime.Parse(timeCurrent);
            var holidays = _holidayRepository.GetAll().Where(x => x.StartDate.Year <= dateCurrent.Year)
                                             .WhereIf(!input.Title.IsNullOrEmpty(), p => p.Title.ToLower().Contains(input.Title.ToLower()))

                                        .OrderBy(input.Sorting)
                                                   .PageBy(input)
                                                   .ToList();

            var count = _holidayRepository.GetAll().Where(x => x.StartDate.Year <= dateCurrent.Year)
                .WhereIf(!input.Title.IsNullOrEmpty(), p => p.Title.ToLower().Contains(input.Title.ToLower()))
                .OrderBy(x => x.StartDate)
                           .Count();
            return new PagedResultDto<HolidayListInputDto>(count, holidays.MapTo<List<HolidayListInputDto>>());
        }

        public async Task UpdateHoliday(EditHoildayDto input)
        {
            try
            {
                var holiday = await _holidayRepository.GetAsync(input.Id);
                holiday.StartDate = input.StartDate;
                holiday.EndDate = input.EndDate;
                holiday.Type = input.Type;
                holiday.Title = input.Title;



                await _holidayRepository.UpdateAsync(holiday);

            }
            catch (Exception)
            {

                
            }
            
            

        }
    }
}
