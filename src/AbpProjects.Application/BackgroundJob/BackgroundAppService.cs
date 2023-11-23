using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Authorization.Users.Dto;
using AbpProjects.InvoiceRequest;
using AbpProjects.MeghPlanningNotification;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.Productions;
using AbpProjects.Project;
using AbpProjects.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpProjects.BackgroundJob
{
    //[AbpAllowAnonymous]
    //[AbpAuthorize(PermissionNames.Pages_Admin_HangfireDashboard)]
    
    public class BackgroundAppService : AbpProjectsApplicationModule, IBackgroundAppService
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<UserNotification> _usernotificationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Production> _productionRepository;
        private readonly IRepository<project> _projectRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<ProjectStatus> _projectStatusRepository;
        private readonly IRepository<timesheet> _timesheetRepository;
        private readonly IRepository<invoicerequest> _invoiceRequestRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        public BackgroundAppService(IRepository<Notification> notificationRepository,
           IRepository<UserNotification> usernotificationRepository, IRepository<User, long> userRepository,
            IRepository<Production> productionRepository,
            IRepository<project> projectRepository, IRepository<Client> clientRepository,
            IRepository<ProjectStatus> projectStatusRepository, IRepository<timesheet> timesheetRepository,
            IRepository<invoicerequest> invoiceRequestRepository, IUserEmailer userEmailer,
            IAbpSession abpSession,
            IRepository<Role> roleRepository, IRepository<UserRole, long> userRoleRepository
           )
        {
            _notificationRepository = notificationRepository;
            _usernotificationRepository = usernotificationRepository;
            _userRepository = userRepository;
            _productionRepository = productionRepository;
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _projectStatusRepository = projectStatusRepository;
            _timesheetRepository = timesheetRepository;
            _invoiceRequestRepository = invoiceRequestRepository;
            _userEmailer = userEmailer;
            _abpSession = abpSession;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task ProjectNotification()
        {
            var userids = _usernotificationRepository.GetAll().Where(x => x.NotificationId == 1).Select(x => x.UserId).ToArray();
            var projectlist = GetReminderProjectList();


            if (projectlist.Count > 0)
            {
                #region sendmail
                GetProjectListDto item = new GetProjectListDto();
                foreach (var id in userids)
                {
                    item.toEmail = _userRepository.GetAll().Where(x => x.Id == id).Select(x => x.EmailAddress).FirstOrDefault();
                    //item.FromEmail = _userRepository.GetAll().Where(x => x.UserName == "admin").Select(x => x.EmailAddress).FirstOrDefault();
                    //item.toEmail = "sneha.patel@meghtechnologies.com";
                    item.EmailTitle = "Project Details";
                    item.ProjectDetails = projectlist;
                    await _userEmailer.SendEmailReminderProjectList(item);
                }
                #endregion
            }

        }

        public List<ProjectDataDto> GetReminderProjectList()
        {
            var actualhours = _timesheetRepository.GetAll()
                        .GroupBy(a => a.ProjectId)
                        .Select(a => new { Hours = a.Sum(b => b.Hours), ProjectId = a.Key })
                        .OrderByDescending(a => a.Hours)
                        .ToList();

            var cc = (from p in _projectRepository.GetAll()
                      join Pro in _productionRepository.GetAll()
                      on p.Id equals Pro.Projectid into productiondata

                      join Invreq in _invoiceRequestRepository.GetAll()
                      on p.Id equals Invreq.ProjectId into projrepo

                      join c in _clientRepository.GetAll()
                      on p.ClientId equals c.Id

                      join s in _projectStatusRepository.GetAll()
                      on p.Status equals s.Id
                      where p.Status == 4 || p.Status == 5 || p.Status == 6 || p.Status == 7 || p.Status == 10 
                      select new ProjectDataDto
                      {
                          Id = p.Id,
                          ProjectName = p.ProjectName,
                          totalhours = p.totalhours,
                          actualhours = 0,
                      }).ToList();

            foreach (var item in cc)
            {
                item.actualhours = actualhours.Where(x => x.ProjectId == item.Id).Select(x => x.Hours).FirstOrDefault();
                item.hourPercentage = item.totalhours > 0 ? ((decimal)actualhours.Where(s => s.ProjectId == item.Id).Select(s => s.Hours).DefaultIfEmpty(0).Sum()) * 100 / item.totalhours : 0;

            }

            //var data = cc.Where(x => x.hourPercentage >= 100).Select(x => x).ToList();
            var data = cc.Where(x => x.hourPercentage >= 60).Select(x => x).OrderByDescending(X=>X.hourPercentage).ToList();
            return data;



        }
    }
}
