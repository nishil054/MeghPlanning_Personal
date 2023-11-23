using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AbpProjects.Authorization.Users;
using AbpProjects.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using AbpProjects.ManageLeaves.Dto;
using Abp.Authorization;
using AbpProjects.Authorization;

namespace AbpProjects.ManageLeaves
{
    
    [AbpAuthorize(PermissionNames.Pages_DataVault_Admin_Leaves)]
    public class ManageLeavesAppService : IManageLeavesAppService
    {
        private readonly IRepository<User, long> _userRepository;

        public ManageLeavesAppService(IRepository<User, long> repository)
        {
            _userRepository = repository;
        }
        //public PagedResultDto<UserDto> GetUserdata(GetUserInputDto Input)
        //{
        //    var userQuery = _userRepository.GetAll().Where(x => x.UserName != "admin" && x.IsActive == true);
        //    var userData = userQuery.OrderByDescending(x => x.Id).ToList();
        //    var userCount = userQuery.Count();
        //    return new PagedResultDto<UserDto>(userCount, userData.MapTo<List<UserDto>>());

        //}

        public List<UserDto> GetUserdata(GetUserInputDto Input)
        {
            List<UserDto> userData = new List<UserDto>();
            var userQuery = _userRepository.GetAll().Where(x => x.UserName != "admin" && x.IsActive == true);
            
            userData = (from og in userQuery
                          
                          select new UserDto
                          {
                              Id = og.Id,
                              UserName = og.UserName,
                              Name = og.Name,
                              Surname = og.Surname,
                              EmailAddress = og.EmailAddress,
                              TeamId = og.TeamId,
                              IsActive = og.IsActive,
                              //FullName = og.FullName,
                              CreationTime = og.CreationTime,
                              CompanyId = og.CompanyId,
                              Salary_Hour = og.Salary_Hour,
                              Salary_Month = og.Salary_Month,
                              LeaveBalance = og.LeaveBalance,
                              PendingLeaves= og.PendingLeaves
                          }).ToList();

            var userCount = userData.Count();
            return userData;

        }

        public async Task UpdateBulkLeaves(List<EmployeeLeaveDto> inputList)
        {
            foreach (var item in inputList) 
            {
                var userData = _userRepository.GetAll().Where(x => x.Id == item.Id).FirstOrDefault();
                if (userData != null)
                {
                    userData.LeaveBalance = item.LeaveBalance;
                    userData.PendingLeaves = item.PendingLeaves;
                    userData.LeaveUpdateDate = DateTime.Now;          
                    await _userRepository.UpdateAsync(userData);
                }
            }
        }
    }
}
