using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using AbpProjects.AuditLogservice.Dto;
using AbpProjects.Authorization.Users;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;

using AbpProjects.AuditLogservice.Dto;

using Abp.UI;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.AuditLogservice
{
   public  class AuditLogsAppService : AbpProjectsApplicationModule, IAuditLogsAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogsRepository;
        private readonly IRepository<User, long> _userRepository;
        public AuditLogsAppService(IRepository<AuditLog, long> auditLogsRepository, IRepository<User, long> userRepository)
        {
            _auditLogsRepository = auditLogsRepository;
            _userRepository = userRepository;
        }
       
        public PagedResultDto<ListData> GetAuditLogs(GetAuditlogInput input)
        {

            var cc = (from p in _auditLogsRepository.GetAll()
                      join u in _userRepository.GetAll()
                      on p.UserId equals u.Id
                      select new ListData
                      {
                          Id = (int)p.Id,
                          ServiceName = p.ServiceName,
                          MethodName = p.MethodName,
                          ExceptionMessage = p.ExceptionMessage,
                          UserName = u.UserName,
                          ExecutionDuration = p.ExecutionDuration,
                          ClientIpAddress = p.ClientIpAddress,
                          ClientName = p.ClientName,
                          BrowserInfo = p.BrowserInfo

                      })
                   .WhereIf(!input.UserName.IsNullOrEmpty() && input.UserName != "", p => p.UserName.ToLower().Contains(input.UserName.ToLower()))
                   .WhereIf(!input.ServiceName.IsNullOrEmpty() && input.ServiceName != "", p => p.ServiceName.ToLower().Contains(input.ServiceName.ToLower()))
                   .WhereIf(!input.MethodName.IsNullOrEmpty() && input.MethodName != "", p => p.MethodName.ToLower().Contains(input.MethodName.ToLower()))
                   .WhereIf(!input.MethodName.IsNullOrEmpty() && input.MethodName != "", p => p.MethodName.ToLower().Contains(input.MethodName.ToLower()))
                   .WhereIf(!input.BrowserInfo.IsNullOrEmpty() && input.BrowserInfo != "", p => p.BrowserInfo.ToLower().Contains(input.BrowserInfo.ToLower()));
                    var ccCount = cc.Count();
                    var ccData = cc.OrderBy(input.Sorting).PageBy(input).ToList();
            
            return new PagedResultDto<ListData>(ccCount, ccData.MapTo<List<ListData>>());
        }
    }
}
