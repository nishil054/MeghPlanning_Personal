using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.LeaveApplication.Dto;
using AbpProjects.LeaveStatus;
using AbpProjects.LeaveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using AbpProjects.Authorization.Users.Dto;
using Abp.Extensions;
using System.Web.UI.WebControls;
using Twilio.Http;

namespace AbpProjects.LeaveApplication
{
    //[AbpAuthorize(PermissionNames.Pages_LeaveApplication, PermissionNames.Pages_LeaveApplicationToDo)]
    [AbpAuthorize]
    public class LeaveApplicationAppService : AbpProjectsApplicationModule, ILeaveApplicationAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Leavetype> _leavetypeRepository;
        private readonly IRepository<Leavestatus> _leavestatusRepository;
        private readonly IRepository<Leaveapplication> _leaveapplicationRepository;
        private readonly IAbpSession _session;
        private readonly IUserEmailer _userEmailer;
        private readonly PermissionChecker _permissionChecker;

        public LeaveApplicationAppService(IRepository<User, long> userRepository, IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,IAbpSession session, IRepository<Leavetype> leavetypeRepository,
            IRepository<Leavestatus> leavestatusRepository, IRepository<Leaveapplication> leaveapplicationRepository,
            IUserEmailer userEmailer, PermissionChecker permissionChecker)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _leavetypeRepository = leavetypeRepository;
            _leavestatusRepository = leavestatusRepository;
            _leaveapplicationRepository = leaveapplicationRepository;
            _session = session;
            _userEmailer = userEmailer;
            _permissionChecker = permissionChecker;
        }

        public List<LeaveTypeDto> GetLeaveType()
        {
            var leavetypelist = (from a in _leavetypeRepository.GetAll()
                               select new LeaveTypeDto
                               {
                                   Id = a.Id,
                                   Type = a.Type,
                               }).OrderBy(x => x.Type).ToList();
            return leavetypelist;
        }

        public List<LeaveStatusDto> GetLeaveStatus()
        {
            var leavestatuslist = (from a in _leavestatusRepository.GetAll()
                                 select new LeaveStatusDto
                                 {
                                     Id = a.Id,
                                     Status = a.Status,
                                 }).OrderBy(x => x.Status).ToList();
            return leavestatuslist;
        }
        public async Task CreateLeave(CreateLeaveDto input)
        {
            int curId = (int)_session.UserId;
            input.UserId = curId;
            var userdata = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x).FirstOrDefault();
            input.Immediate_supervisorId = userdata.Immediate_supervisorId;
            input.LeaveBalance = userdata.LeaveBalance;
            input.LeaveStatus = 1;      //Pending
            input.PendingLeave = userdata.PendingLeaves;
            var result = input.MapTo<Leaveapplication>();

            #region sendmail
            GetUserListDto item = new GetUserListDto();
            var roleId = _roleRepository.GetAll().Where(x => x.Name == "HR").Select(x => x.Id).FirstOrDefault();
            var userId = _userRoleRepository.GetAll().Where(x => x.RoleId == roleId).Select(x => x.UserId).FirstOrDefault();
            if (roleId != null && userId != null)
            {
                item.CC = _userRepository.GetAll().Where(x => x.Id == userId).Select(x => x.EmailAddress).FirstOrDefault();
            }
            if (input.Immediate_supervisorId != null) 
            { 
                item.toEmail = _userRepository.GetAll().Where(x => x.Id == input.Immediate_supervisorId).Select(x => x.EmailAddress).FirstOrDefault();
            }
            else
            {
                item.toEmail = _userRepository.GetAll().Where(x => x.UserName == "admin").Select(x => x.EmailAddress).FirstOrDefault();
            }
            item.FromEmail = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.EmailAddress).FirstOrDefault();

            item.UserId = curId;
            item.UserName = userdata.Name + " " + userdata.Surname;
            item.FromDate = input.FromDate.ToString("dd-MM-yyyy");
            item.ToDate = input.ToDate.ToString("dd-MM-yyyy");
            item.EmailTitle = "Leave Application Request for Employee "+ item.UserName;
            item.Reason = input.Reason;
            item.LeaveType = _leavetypeRepository.GetAll().Where(x=>x.Id == input.LeaveType).Select(x=>x.Type).FirstOrDefault();
            try {
                await _userEmailer.SendLeaveApplicationEmail(item);
            } catch { }
          
            #endregion

            await _leaveapplicationRepository.InsertAsync(result);
        }

        public PagedResultDto<LeaveDto> GetLeaveData(GetInputDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            int curId = (int)_session.UserId;
            
            var data = (from a in _leaveapplicationRepository.GetAll()
                        join b in _userRepository.GetAll()
                        on a.UserId equals (int)b.Id
                        join c in _leavetypeRepository.GetAll()
                        on a.LeaveType equals c.Id
                        join d in _leavestatusRepository.GetAll()
                        on a.LeaveStatus equals d.Id
                        select new LeaveDto
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name + " " + b.Surname,
                            LeaveType = a.LeaveType,
                            LeaveTypeName = c.Type,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                            Reason = a.Reason,
                            LeaveStatus = a.LeaveStatus,
                            LeaveStatusName = d.Status,
                        })
                        .WhereIf(!input.Filter.IsNullOrEmpty(), p => p.UserName.ToLower().Contains(input.Filter.ToLower()))
                        .WhereIf(input.FromDate.HasValue, p => p.FromDate >= dtfrm)
                        .WhereIf(input.ToDate.HasValue, p => p.ToDate <= dt)
                        .WhereIf(input.LeaveStatusId != null, p => p.LeaveStatus == input.LeaveStatusId);

            if (_permissionChecker.IsGranted("Pages.User.LeaveApplication"))
            {
                data = data.Where(x => x.UserId == curId);
            }

            var ccData = data.OrderBy(input.Sorting).PageBy(input).ToList();

            var ccCount = data.Count();

            return new PagedResultDto<LeaveDto>(ccCount, ccData.MapTo<List<LeaveDto>>());
        }

        public PagedResultDto<LeaveDto> GetLeaveDataReport(GetInputDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            int curId = (int)_session.UserId;

            var data = (from a in _leaveapplicationRepository.GetAll()
                        join b in _userRepository.GetAll()
                        on a.UserId equals (int)b.Id
                        join c in _leavetypeRepository.GetAll()
                        on a.LeaveType equals c.Id
                        join d in _leavestatusRepository.GetAll()
                        on a.LeaveStatus equals d.Id
                        select new LeaveDto
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name + " " + b.Surname,
                            LeaveType = a.LeaveType,
                            LeaveTypeName = c.Type,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                            Reason = a.Reason,
                            LeaveStatus = a.LeaveStatus,
                            LeaveStatusName = d.Status,
                        })
                        .WhereIf(input.UserId != null, p => p.UserId == input.UserId)
                        .WhereIf(input.FromDate.HasValue, p => p.FromDate >= dtfrm)
                        .WhereIf(input.ToDate.HasValue, p => p.ToDate <= dt)
                        .WhereIf(input.LeaveStatusId != null, p => p.LeaveStatus == input.LeaveStatusId);

            if (_permissionChecker.IsGranted("Pages.User.LeaveApplication"))
            {
                //data = data.Where(x => x.UserId == curId);
            }

            var ccData = data.OrderBy(input.Sorting).PageBy(input).ToList();

            var ccCount = data.Count();

            return new PagedResultDto<LeaveDto>(ccCount, ccData.MapTo<List<LeaveDto>>());
        }

        public List<LeaveDto> GetLeaveDataReportExport(GetInputDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            int curId = (int)_session.UserId;

            var data = (from a in _leaveapplicationRepository.GetAll()
                        join b in _userRepository.GetAll()
                        on a.UserId equals (int)b.Id
                        join c in _leavetypeRepository.GetAll()
                        on a.LeaveType equals c.Id
                        join d in _leavestatusRepository.GetAll()
                        on a.LeaveStatus equals d.Id
                        select new LeaveDto
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name + " " + b.Surname,
                            LeaveType = a.LeaveType,
                            LeaveTypeName = c.Type,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                            Reason = a.Reason,
                            LeaveStatus = a.LeaveStatus,
                            LeaveStatusName = d.Status,
                        })
                        .WhereIf(input.UserId != null, p => p.UserId == input.UserId)
                        .WhereIf(input.FromDate.HasValue, p => p.FromDate >= dtfrm)
                        .WhereIf(input.ToDate.HasValue, p => p.ToDate <= dt)
                        .WhereIf(input.LeaveStatusId != null, p => p.LeaveStatus == input.LeaveStatusId);

            if (_permissionChecker.IsGranted("Pages.User.LeaveApplication"))
            {
                //data = data.Where(x => x.UserId == curId);
            }

            //var ccData = data.OrderBy(input.Sorting).PageBy(input).ToList();
            var ccData = data.ToList();

            var ccCount = data.Count();

            return new List<LeaveDto>( ccData.MapTo<List<LeaveDto>>());
        }

        public ListResultDto<LeaveDto> GetLeaveDataById(EntityDto input)
        {
            var data = (from a in _leaveapplicationRepository.GetAll()
                        join b in _userRepository.GetAll()
                        on a.UserId equals (int)b.Id
                        join c in _leavetypeRepository.GetAll()
                        on a.LeaveType equals c.Id
                        join d in _leavestatusRepository.GetAll()
                        on a.LeaveStatus equals d.Id
                        where a.Id == input.Id 
                        select new LeaveDto
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name + " " + b.Surname,
                            LeaveType = a.LeaveType,
                            LeaveTypeName = c.Type,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                            Reason = a.Reason,
                            LeaveStatus = a.LeaveStatus,
                            LeaveStatusName = d.Status,
                        }).ToList();
            
            return new ListResultDto<LeaveDto>(data.MapTo<List<LeaveDto>>());
        }

        public async Task UpdateLeaveCancelRequest(UpdateStatusDto input)
        {
            int curId = (int)_session.UserId;
            var data = await _leaveapplicationRepository.GetAsync(input.Id);
            var userdata = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x).FirstOrDefault();

            if (input.LeaveStatus == 1)  //Pending
            {
                data.LeaveStatus = 6;  //Cancelled
            }
            else if(input.LeaveStatus == 2)  //Approve
            {
                data.LeaveStatus = 4; //Leave Cancel Request 
            }
            else
            {
                //data.LeaveStatus = 4; //Leave Cancel Request 
            }

            #region sendmail
            GetUserListDto item = new GetUserListDto();
            var roleId = _roleRepository.GetAll().Where(x => x.Name == "HR").Select(x => x.Id).FirstOrDefault();
            var userId = _userRoleRepository.GetAll().Where(x => x.RoleId == roleId).Select(x => x.UserId).FirstOrDefault();
            if (roleId != null && userId != null)
            {
                item.CC = _userRepository.GetAll().Where(x => x.Id == userId).Select(x => x.EmailAddress).FirstOrDefault();
            }
            if (data.Immediate_supervisorId != null)
            {
                item.toEmail = _userRepository.GetAll().Where(x => x.Id == data.Immediate_supervisorId).Select(x => x.EmailAddress).FirstOrDefault();
            }
            else
            {
                item.toEmail = _userRepository.GetAll().Where(x => x.UserName == "admin").Select(x => x.EmailAddress).FirstOrDefault();
            }
            item.FromEmail = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.EmailAddress).FirstOrDefault();
            item.UserId = curId;
            item.UserName = userdata.Name + " " + userdata.Surname;
            item.FromDate = data.FromDate.ToString("dd-MM-yyyy");
            item.ToDate = data.ToDate.ToString("dd-MM-yyyy");
            item.EmailTitle = input.LeaveStatus == 1 ? "Leave Cancelled" : "Leave Cancel Request Approved";
            item.Reason = data.Reason;
            item.LeaveType = _leavetypeRepository.GetAll().Where(x => x.Id == data.LeaveType).Select(x => x.Type).FirstOrDefault();
            try
            {
                await _userEmailer.SendLeaveApplicationCancellationEmail(item);
            }
            catch { }

            #endregion

            await _leaveapplicationRepository.UpdateAsync(data);
        }

        //Leave Application TO Do

        public PagedResultDto<LeaveDto> GetLeaveToDoData(GetInputDto input)
        {
            var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
            DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

            var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
            DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

            int curId = (int)_session.UserId;
            string roleName = "";
            var roleId = _userRoleRepository.GetAll().Where(x => x.UserId == curId).Select(x => x.RoleId).FirstOrDefault();
            if (roleId != null)
            {
                roleName = _roleRepository.GetAll().Where(x => x.Id == roleId).Select(x => x.Name).FirstOrDefault();
            }

            var data = (from a in _leaveapplicationRepository.GetAll()
                        join b in _userRepository.GetAll()
                        on a.UserId equals (int)b.Id
                        join c in _leavetypeRepository.GetAll()
                        on a.LeaveType equals c.Id
                        join d in _leavestatusRepository.GetAll()
                        on a.LeaveStatus equals d.Id
                        where a.LeaveStatus == 1  || a.LeaveStatus == 4 //Pending,Leave Cancel Request
                        select new LeaveDto
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Immediate_supervisorId = b.Immediate_supervisorId.HasValue ? b.Immediate_supervisorId.Value : 0,
                            UserName = b.Name + " " + b.Surname,
                            LeaveType = a.LeaveType,
                            LeaveTypeName = c.Type,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                            Reason = a.Reason,
                            LeaveStatus = a.LeaveStatus,
                            LeaveStatusName = d.Status,
                            RoleName = roleName,
                        })
                        .WhereIf(!input.Filter.IsNullOrEmpty(), p => p.UserName.ToLower().Contains(input.Filter.ToLower()))
                        .WhereIf(input.FromDate.HasValue, p => p.FromDate >= dtfrm)
                        .WhereIf(input.ToDate.HasValue, p => p.ToDate <= dt)
                        .WhereIf(input.LeaveStatusId != null, p => p.LeaveStatus == input.LeaveStatusId);

            if (_permissionChecker.IsGranted("Pages.User.LeaveApplicationToDo"))
            {
                data = data.Where(x => x.Immediate_supervisorId == curId || roleName == "HR");
                //data = data.Where(x => x.RoleName == "HR");
            }

            var ccData = data.OrderBy(input.Sorting).PageBy(input).ToList();

            var ccCount = data.Count();

            return new PagedResultDto<LeaveDto>(ccCount, ccData.MapTo<List<LeaveDto>>());
        }
        public async Task ApproveLeaveRequest(EntityDto input)
        {
            int curId = (int)_session.UserId;
            var data = await _leaveapplicationRepository.GetAsync(input.Id);
            var userdata = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x).FirstOrDefault();
            int status = 0;
            if (data.LeaveStatus == 1) //Approve Leave
            {
                status = 2;
            }
            if (data.LeaveStatus == 4) //Cancel Request Leave
            {
                status = 6;
            }
            data.LeaveStatus = status;

            #region sendmail
            GetUserListDto item = new GetUserListDto();
            var roleId = _roleRepository.GetAll().Where(x => x.Name == "HR").Select(x => x.Id).FirstOrDefault();
            var userId = _userRoleRepository.GetAll().Where(x => x.RoleId == roleId).Select(x => x.UserId).FirstOrDefault();
            if (roleId != null && userId != null)
            {
                item.CC = _userRepository.GetAll().Where(x => x.Id == userId).Select(x => x.EmailAddress).FirstOrDefault();
            }

            item.toEmail = _userRepository.GetAll().Where(x => x.Id == data.CreatorUserId).Select(x => x.EmailAddress).FirstOrDefault();
            item.FromEmail = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.EmailAddress).FirstOrDefault();

            item.UserId = curId;
            item.UserName = userdata.Name + " " + userdata.Surname;
            item.FromDate = data.FromDate.ToString("dd-MM-yyyy");
            item.ToDate = data.ToDate.ToString("dd-MM-yyyy");
            item.EmailTitle = data.LeaveStatus == 2 ? "Leave Application Approved" : "Leave Cancel Request Approved";
            item.Reason = data.Reason;
            item.LeaveType = _leavetypeRepository.GetAll().Where(x => x.Id == data.LeaveType).Select(x => x.Type).FirstOrDefault();
            try
            {
                await _userEmailer.SendLeaveApplicationApprovalEmail(item);
            }
            catch { }

            #endregion

            await _leaveapplicationRepository.UpdateAsync(data);

        }

        public async Task RejectLeaveRequest(EntityDto input)
        {
            int curId = (int)_session.UserId;
            var data = await _leaveapplicationRepository.GetAsync(input.Id);
            var userdata = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x).FirstOrDefault();
            int status = 0;
            if (data.LeaveStatus == 1) //Reject
            {
                status = 3;
            }
            if (data.LeaveStatus == 4) //Cancel Request Approve
            {
                status = 5;
            }
            data.LeaveStatus = status;

            #region sendmail
            GetUserListDto item = new GetUserListDto();
            var roleId = _roleRepository.GetAll().Where(x => x.Name == "HR").Select(x => x.Id).FirstOrDefault();
            var userId = _userRoleRepository.GetAll().Where(x => x.RoleId == roleId).Select(x => x.UserId).FirstOrDefault();
            if (roleId != null && userId != null)
            {
                item.CC = _userRepository.GetAll().Where(x => x.Id == userId).Select(x => x.EmailAddress).FirstOrDefault();
            }

            item.toEmail = _userRepository.GetAll().Where(x => x.Id == data.CreatorUserId).Select(x => x.EmailAddress).FirstOrDefault();
            item.FromEmail = _userRepository.GetAll().Where(x => x.Id == curId).Select(x => x.EmailAddress).FirstOrDefault();

            item.UserId = curId;
            item.UserName = userdata.Name + " " + userdata.Surname;
            item.FromDate = data.FromDate.ToString("dd-MM-yyyy");
            item.ToDate = data.ToDate.ToString("dd-MM-yyyy");
            item.EmailTitle = data.LeaveStatus == 3 ? "Leave Application Rejected" : "Leave Cancel Request Rejected";
            item.Reason = data.Reason;
            item.LeaveType = _leavetypeRepository.GetAll().Where(x => x.Id == data.LeaveType).Select(x => x.Type).FirstOrDefault();
            try
            {
                await _userEmailer.SendLeaveApplicationRejectionEmail(item);
            }
            catch { }

            #endregion

            await _leaveapplicationRepository.UpdateAsync(data);
        }
    }
}
