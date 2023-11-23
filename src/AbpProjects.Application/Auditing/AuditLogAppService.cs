using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AbpProjects.Auditing.Dto;
//using AbpProjects.Auditing.Exporting;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Users;
using AbpProjects.Dto;
using System.Linq.Dynamic.Core;
using System;
using AbpProjects.AuditLogservice.Dto;

namespace AbpProjects.Auditing
{
    [DisableAuditing]
    [AbpAuthorize(PermissionNames.Pages_Administration_AuditLogs)]
    public class AuditLogAppService : AbpProjectsAppServiceBase, IAuditLogAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<User, long> _userRepository;
        //private readonly IAuditLogListExcelExporter _auditLogListExcelExporter;
        private readonly INamespaceStripper _namespaceStripper;
    
        public AuditLogAppService(
            IRepository<AuditLog, long> auditLogRepository, 
            IRepository<User, long> userRepository, 
            //IAuditLogListExcelExporter auditLogListExcelExporter, 
            INamespaceStripper namespaceStripper)
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            //_auditLogListExcelExporter = auditLogListExcelExporter;
            _namespaceStripper = namespaceStripper;
        }
        
        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            var query = CreateAuditLogAndUsersQuery(input);

            var resultCount =   query.Count();
            var results =   query/*.Distinct()*/
                //.AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToList();

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            return new PagedResultDto<AuditLogListDto>(resultCount, auditLogListDtos);
        }

        //public async Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input)
        //{
        //    var auditLogs = await CreateAuditLogAndUsersQuery(input)
        //                .AsNoTracking()
        //                .OrderByDescending(al => al.AuditLog.ExecutionTime)
        //                .ToListAsync();

        //    var auditLogListDtos = ConvertToAuditLogListDtos(auditLogs);

        //    return _auditLogListExcelExporter.ExportToFile(auditLogListDtos);
        //}

        private List<AuditLogListDto> ConvertToAuditLogListDtos(List<AuditLogAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var auditLogListDto = result.AuditLog.MapTo<AuditLogListDto>();
                    auditLogListDto.UserName = result.User == null ? null : result.User.UserName;
                    auditLogListDto.ServiceName = _namespaceStripper.StripNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }

        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            var frmdate = input.StartDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.StartDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");
          
            var todate = input.EndDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.EndDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");
            
            var query = from auditLog in _auditLogRepository.GetAll()
                join user in _userRepository.GetAll() on auditLog.UserId equals user.Id into userJoin
                from joinedUser in userJoin.DefaultIfEmpty()
                where auditLog.ExecutionTime >= dtfrm && auditLog.ExecutionTime <= dt
                        select new AuditLogAndUser {AuditLog = auditLog, User = joinedUser};

            query = query
                 .WhereIf(!input.UserName.IsNullOrEmpty() && input.UserName != "", p => p.User.UserName.ToLower().Contains(input.UserName.ToLower()))
                //.WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.User.UserName.Contains(input.UserName))
                .WhereIf(!input.ServiceName.IsNullOrEmpty() && input.ServiceName != "", p => p.AuditLog.ServiceName.ToLower().Contains(input.ServiceName.ToLower()))
                 //.WhereIf(!input.ServiceName.IsNullOrWhiteSpace(), item => item.AuditLog.ServiceName.Contains(input.ServiceName))
                //.WhereIf(!input.MethodName.IsNullOrWhiteSpace(), item => item.AuditLog.MethodName.Contains(input.MethodName))
                .WhereIf(!input.MethodName.IsNullOrEmpty() && input.MethodName != "", p => p.AuditLog.MethodName.ToLower().Contains(input.MethodName.ToLower()))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(), item => item.AuditLog.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0, item => item.AuditLog.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue, item => item.AuditLog.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true, item => item.AuditLog.Exception != null && item.AuditLog.Exception != "")
                .WhereIf(input.HasException == false, item => item.AuditLog.Exception == null || item.AuditLog.Exception == "");
            return query;
        }

        

    }
}
