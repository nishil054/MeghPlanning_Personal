using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.InvoiceRequest;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.MeghPlanningSupportServices.Dto;
using AbpProjects.Project.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AbpProjects.Authorization.Users.Dto;
using System.Globalization;
using AbpProjects.Notifications;
using Abp.Notifications;

namespace AbpProjects.MeghPlanningSupportServices
{
    [AbpAuthorize(PermissionNames.Pages_SupportManageService, PermissionNames.Pages_Project, PermissionNames.Page_Support)]
    public class SupportAppService : AbpProjectsCoreModule, ISupportAppService
    {

        private readonly IRepository<ManageService> _manageserviceRepository;
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<ServerTypeDetail> _serverRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Typename> _typenameRepository;
        private readonly IRepository<ServiceRequestHistory> _serviceRequestRepository;
        private readonly IRepository<invoicerequest> _invoiceRequestRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly IAppNotifier _appNotifier;
        public SupportAppService(IRepository<ManageService> manageserviceRepository, IRepository<Service> serviceRepository, IRepository<Client> clientRepository, IRepository<User, long> userRepository, IRepository<ServerTypeDetail> serverRepository, IRepository<Typename> typenameRepository, IRepository<ServiceRequestHistory> serviceRequestRepository, IRepository<invoicerequest> invoiceRequestRepository,
            IRepository<UserRole, long> userRoleRepository, IRepository<Role> roleRepository, IUserEmailer userEmailer, IAppNotifier appNotifier)
        {
            _manageserviceRepository = manageserviceRepository;
            _serviceRepository = serviceRepository;
            _clientRepository = clientRepository;
            _serverRepository = serverRepository;
            _userRepository = userRepository;
            _typenameRepository = typenameRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _invoiceRequestRepository = invoiceRequestRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userEmailer = userEmailer;
            _appNotifier = appNotifier;
        }
        //[AbpAuthorize(PermissionNames.Pages_SupportManageService)]
        public async Task CreateService(InsertDataDto input)
        {
            if ((input.NextRenewalDate <= DateTime.Today))
            {
                throw new UserFriendlyException("Renewal Date is not Valid");
            }
            if (input.ServiceId == 1)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.TypeName = null;
                input.NoOfEmail = null;
                input.ServerType = null;
                input.Title = null;
                input.Typeofssl = null;
            }
            if (input.ServiceId == 2 || input.ServiceId == 3)
            {

                input.TypeName = null;
                input.NoOfEmail = null;
                input.Title = null;
                input.Typeofssl = null;
            }
            if (input.ServiceId == 8)
            {

                input.TypeName = null;
                input.NoOfEmail = null;
                input.Title = null;
                input.Typeofssl = null;
                input.HostingSpace = null;
            }
            if (input.ServiceId == 4)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.Title = null;
                input.Typeofssl = null;
                //input.ServerType = null;
            }

            if (input.ServiceId == 5)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.TypeName = null;
                input.NoOfEmail = null;
                input.ServerType = null;
                input.Title = null;

            }
            if (input.ServiceId == 6)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.TypeName = null;
                input.NoOfEmail = null;
                input.ServerType = null;
                input.Typeofssl = null;

            }

            input.Cancelflag = false;


            var service = input.MapTo<ManageService>();

            await _manageserviceRepository.InsertAndGetIdAsync((ManageService)service);


            //Insert into child table for service history 

            ServiceRequestHistory cd = new ServiceRequestHistory();
            cd.ServiceId = service.Id;
            cd.Amount = service.Price;
            cd.AdjustmentAmount = input.AdjustmentAmount;
            cd.Comment = service.Comment;
            cd.Actiontype = 1;
            cd.RegistrationDate = input.RegistrationDate;
            cd.NextRenewalDate = input.NextRenewalDate;
            cd.ActionName = "New";
            cd.RegistrationDate = input.RegistrationDate;
            cd.NextRenewalDate = input.NextRenewalDate;
            cd.InvoiceNote = input.InvoiceNote;
            cd.Period = Convert.ToDateTime(input.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(input.NextRenewalDate).ToString("dd/MM/yyyy");

            var ServiceReqId = cd.MapTo<ServiceRequestHistory>();
            await _serviceRequestRepository.InsertAndGetIdAsync((ServiceRequestHistory)cd);

            if (input.solution == "Y")
            {
                invoicerequest Invreqobj = new invoicerequest();
                Invreqobj.ServiceId = service.Id;
                Invreqobj.ServiceReqId = ServiceReqId.Id;
                Invreqobj.TypeId = 1;
                Invreqobj.Comment = input.InvoiceNote;
                Invreqobj.Status = 0;
                Invreqobj.Amount = input.Price;
                Invreqobj.InvoiceNote = input.InvoiceNote;
                Invreqobj.Period = Convert.ToDateTime(input.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(input.NextRenewalDate).ToString("dd/MM/yyyy");
                var InvId = Invreqobj.MapTo<invoicerequest>();
                await _invoiceRequestRepository.InsertAsync((invoicerequest)Invreqobj);
            }

            //Insert into InvoiceRequest table

        }
        //[AbpAuthorize(PermissionNames.Pages_SupportManageService,PermissionNames.Page_Support)]
        public PagedResultDto<ListDataDto> GetServiceMgt(GetServiceInput input)
        {
            try
            {

                var frmdate = input.FromDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.FromDate.Value.ToString("MM/dd/yyyy");
                DateTime dtfrm = Convert.ToDateTime(frmdate + " 00:00:00");

                var todate = input.ToDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : input.ToDate.Value.ToString("MM/dd/yyyy");
                DateTime dt = Convert.ToDateTime(todate + " 23:59:59");

                var tasks = (from e1 in _manageserviceRepository.GetAll()
                             join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                              into distservice
                             join e7 in _invoiceRequestRepository.GetAll()
                             on e1.Id equals e7.ServiceId
                             into invoicerequest
                             from e2 in distservice.DefaultIfEmpty()
                             join e3 in _clientRepository.GetAll() on e1.ClientId equals e3.Id
                              into distclient
                             from e3 in distclient.DefaultIfEmpty()
                             join e4 in _userRepository.GetAll() on e1.EmployeeId equals e4.Id
                              into distuser
                             from e4 in distuser.DefaultIfEmpty()
                             join e5 in _serverRepository.GetAll() on e1.ServerType equals e5.Id
                              into distserver
                             from e5 in distserver.DefaultIfEmpty()
                             join e6 in _typenameRepository.GetAll() on e1.TypeName equals e6.Id
                               into distrtype
                             from e6 in distrtype.DefaultIfEmpty()
                             join e7 in _serviceRequestRepository.GetAll() on e1.Id equals e7.ServiceId
                               into distst
                             from e7 in distst.DefaultIfEmpty()
                                 //where (e1.Cancelflag == false)
                             select new ListDataDto
                             {
                                 Id = e1.Id,
                                 DomainName = e1.DomainName,
                                 Price = e1.Price,
                                 HostingSpace = e1.HostingSpace,
                                 ServiceId = e1.ServiceId,
                                 ServiceName = e2.Name,
                                 NextRenewalDate = e1.NextRenewalDate,
                                 ClientId = e1.ClientId,
                                 ClientName = e3.ClientName,
                                 EmployeeId = e1.EmployeeId,
                                 EmployeeName = e4.Name + " " + e4.Surname,
                                 ServerName = e5.Name,
                                 TypeName = e1.TypeName,
                                 Typeofssl = e1.Typeofssl,
                                 Title = e1.Title,
                                 NoOfEmail = e1.NoOfEmail,
                                 Cancelflag = e1.Cancelflag,
                                 DisplayTypename = e6.Name,
                                 ActionName = distst.OrderByDescending(x => x.Id).Select(x => x.ActionName).FirstOrDefault(),
                                 RegistrationDate = e1.RegistrationDate,
                                 StatusName = distst.OrderByDescending(x => x.Id).Select(x => x.Actiontype).FirstOrDefault(),
                                 Status = invoicerequest.OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault(),
                                 Credits = e1.Credits,
                                 DatabaseSpace = e1.DatabaseSpace,
                                 Comment = e1.Comment,
                                 //AdjustmentAmount=e1.AdjustmentAmount
                                IsAutoRenewal = e1.IsAutoRenewal,

                             })
                             .WhereIf(input.ServiceId.HasValue, x => x.ServiceId == input.ServiceId)
                             .WhereIf(input.TypeName.HasValue, x => x.TypeName == input.TypeName)
                             .WhereIf(input.ClientId.HasValue, x => x.ClientId == input.ClientId)
                             .WhereIf(input.EmployeeId.HasValue, x => x.EmployeeId == input.EmployeeId)
                             .WhereIf(input.Cancelflag.HasValue, x => x.Cancelflag == input.Cancelflag)
                            .WhereIf(!input.DomainName.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(input.DomainName.ToLower()))
                             .WhereIf(input.FromDate.HasValue, p => p.NextRenewalDate >= dtfrm)
                              .WhereIf(input.ToDate.HasValue, p => p.NextRenewalDate <= dt).Distinct();
                var totalcount = tasks.Count();
                var servicePageBy = tasks.OrderBy(input.Sorting).ThenBy(x => x.DomainName).PageBy(input).ToList();
                return new PagedResultDto<ListDataDto>(totalcount, servicePageBy.MapTo<List<ListDataDto>>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ListResultDto<ListDataDto> GetServiceName(GetServiceInput input)
        {
            var persons = (from item in _serviceRepository
                 .GetAll()
                           select new ListDataDto
                           {
                               ServiceId = item.Id,
                               ServiceName = item.Name
                           })
                           .OrderBy(x => x.ServiceName).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }

        public ListResultDto<ListDataDto> GetClientName(GetServiceInput input)
        {
            var persons = (from item in _clientRepository
                 .GetAll()
                           select new ListDataDto
                           {
                               ClientId = item.Id,
                               ClientName = item.ClientName
                           })
                           .OrderBy(x => x.ClientName).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }
        public ListResultDto<ListDataDto> GetTypeName(GetServiceInput input)
        {
            var persons = (from item in _typenameRepository
                 .GetAll()
                           select new ListDataDto
                           {

                               TypeName = item.Id,
                               DisplayTypename = item.Name
                           })
                           .OrderBy(x => x.DisplayTypename).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }
        public ListResultDto<ListDataDto> GetEmployeeName(GetServiceInput input)
        {
            var persons = (from item in _userRepository
                 .GetAll()
                           select new ListDataDto
                           {
                               EmployeeId = (int)item.Id,
                               EmployeeName = item.Name + " " + item.Surname
                           })
                           .OrderBy(x => x.EmployeeName).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }
        //[AbpAuthorize(PermissionNames.Pages_SupportManageService)]
        public async Task<ListDataDto> GetServiceForEdit(int id)
        {
            //ListDataDto d = new ListDataDto();
            //if (d.checkrenew == false && d.checkadjustment == false) {
            //}
            var tasks = (from e1 in _manageserviceRepository.GetAll()
                         join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                         join e3 in _clientRepository.GetAll() on e1.ClientId equals e3.Id
                         into distrclient
                         from e3 in distrclient.DefaultIfEmpty()
                         join e4 in _userRepository.GetAll() on e1.EmployeeId equals e4.Id
                         into distruser
                         from e4 in distruser.DefaultIfEmpty()
                         join e5 in _serverRepository.GetAll() on e1.ServerType equals e5.Id
                          into distrrole
                         from e5 in distrrole.DefaultIfEmpty()
                         join e6 in _typenameRepository.GetAll() on e1.TypeName equals e6.Id
                          into distrtype
                         from e6 in distrtype.DefaultIfEmpty()
                         where (e1.Id == id)
                         select new ListDataDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             Price = e1.Price,
                             NextRenewalDate = e1.NextRenewalDate,
                             Comment = e1.Comment,
                             HostingSpace = e1.HostingSpace,
                             ServerType = e1.ServerType,
                             TypeName = e1.TypeName,
                             NoOfEmail = e1.NoOfEmail,
                             ServiceId = e1.ServiceId,
                             ClientId = e1.ClientId,
                             EmployeeId = e1.EmployeeId,
                             ServiceName = e2.Name,
                             ClientName = e3.ClientName,
                             EmployeeName = e4.UserName,
                             ServerName = e5.Name,
                             DisplayTypename = e6.Name,
                             Typeofssl = e1.Typeofssl,
                             Title = e1.Title,
                             RegistrationDate = e1.RegistrationDate,
                             Credits = e1.Credits,
                             DatabaseSpace = e1.DatabaseSpace,
                             IsAutoRenewal = e1.IsAutoRenewal,
                             Term = e1.Term,
                             //AdjustmentAmount = e1.AdjustmentAmount
                         })
                     .FirstOrDefault();

            return tasks;
        }

        public ListResultDto<ListDataDto> GetServerName(GetServiceInput input)
        {
            var persons = (from item in _serverRepository
                 .GetAll()
                           select new ListDataDto
                           {
                               ServerType = item.Id,
                               ServerName = item.Name
                           })
                           .OrderBy(x => x.ServerType).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }

        public ListResultDto<ListDataDto> GetNextRenewqldate(GetServiceInput input)
        {
            var persons = (from item in _manageserviceRepository
                 .GetAll()
                           select new ListDataDto
                           {
                               Id = item.Id,
                               NextRenewalDate = item.NextRenewalDate.AddYears(1)
                           })
                           .OrderBy(x => x.ServerName).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }
        //[AbpAuthorize(PermissionNames.Pages_SupportManageService)]
        public async Task UpdateService(UpdateDataDto input)
        {
            try
            {
                if ((input.NextRenewalDate < DateTime.Today))
                {
                    throw new UserFriendlyException("Renewal Date is not Valid");
                }

                if (input.ServiceId == 1)
                {
                    input.HostingSpace = null;
                    input.DatabaseSpace = null;
                    input.TypeName = null;
                    input.NoOfEmail = null;
                    input.ServerType = null;
                    input.Title = null;
                    input.Typeofssl = null;
                }
                if (input.ServiceId == 2 || input.ServiceId == 3)
                {

                    input.TypeName = null;
                    input.NoOfEmail = null;
                    input.Title = null;
                    input.Typeofssl = null;
                }
                if (input.ServiceId == 8)
                {
                    input.TypeName = null;
                    input.NoOfEmail = null;
                    input.Title = null;
                    input.Typeofssl = null;
                    input.HostingSpace = null;

                }
                if (input.ServiceId == 4)
                {
                    input.HostingSpace = null;
                    input.DatabaseSpace = null;
                    input.Title = null;
                    input.Typeofssl = null;
                    // input.ServerType = null;
                }
                if (input.ServiceId == 5)
                {
                    input.HostingSpace = null;
                    input.DatabaseSpace = null;
                    input.TypeName = null;
                    input.NoOfEmail = null;
                    input.ServerType = null;
                    input.Title = null;

                }
                if (input.ServiceId == 6)
                {
                    input.HostingSpace = null;
                    input.DatabaseSpace = null;
                    input.TypeName = null;
                    input.NoOfEmail = null;
                    input.ServerType = null;
                    input.Typeofssl = null;

                }
                var per = await _manageserviceRepository.FirstOrDefaultAsync(input.Id);
                var serviceRquestID = _serviceRequestRepository.GetAll().Where(e => e.ServiceId == per.Id).OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();

                per.DomainName = input.DomainName;
                per.Price = input.Price;
                per.NextRenewalDate = input.NextRenewalDate;
                per.IsAutoRenewal = input.IsAutoRenewal;
                per.Comment = input.Comment;
                per.HostingSpace = input.HostingSpace;
                per.DatabaseSpace = input.DatabaseSpace;
                per.ServerType = input.ServerType;
                per.TypeName = input.TypeName;
                per.NoOfEmail = input.NoOfEmail;
                per.EmployeeId = input.EmployeeId;
                per.ServiceId = input.ServiceId;
                per.ClientId = input.ClientId;
                per.RegistrationDate = input.RegistrationDate;
                per.Title = input.Title;
                per.Typeofssl = input.Typeofssl;
                per.Credits = input.Credits;
                per.Term = input.Term;
                if (input.Cancelflag != true)
                {
                    per.RenewalDate = DateTime.Now;
                    per.Cancelflag = false;
                }

                await _manageserviceRepository.UpdateAsync(per);

                if (input.checkrenew == true)
                {
                    ServiceRequestHistory cd = new ServiceRequestHistory();
                    //cd.Id = serviceRquestID;
                    cd.ServiceId = per.Id;
                    cd.Amount = input.Price;
                    cd.AdjustmentAmount = input.AdjustmentAmount;
                    cd.Comment = input.Comment;
                    cd.Actiontype = 2;
                    cd.ActionName = "Renew";
                    cd.RegistrationDate = input.RegistrationDate;
                    cd.NextRenewalDate = input.NextRenewalDate;
                    //cd.InvoiceNote = input.InvoiceNote;
                    //var ServiceReqId = cd.MapTo<ServiceRequestHistory>();
                    //var servicereqId= cd.MapTo<ServiceRequestHistory>();
                    await _serviceRequestRepository.InsertAndGetIdAsync((ServiceRequestHistory)cd);


                    //invoicerequest Invreqobj = new invoicerequest();
                    //Invreqobj.ServiceId = per.Id;
                    //Invreqobj.ServiceReqId = cd.Id;
                    //Invreqobj.TypeId = 1;
                    //Invreqobj.Comment = input.Comment;
                    //Invreqobj.Status = 0;
                    //Invreqobj.Amount = input.Price;
                    ////Invreqobj.InvoiceNote = cd.InvoiceNote;
                    //var InvId = Invreqobj.MapTo<invoicerequest>();
                    //await _invoiceRequestRepository.InsertAsync((invoicerequest)Invreqobj);
                }
                if (input.checkrenewDashboard == true)
                {
                    ServiceRequestHistory cd = new ServiceRequestHistory();
                    cd.Id = serviceRquestID;
                    cd.ServiceId = per.Id;
                    cd.Amount = input.Price;
                    cd.AdjustmentAmount = input.AdjustmentAmount;
                    cd.Comment = input.Comment;
                    cd.Actiontype = 2;
                    cd.ActionName = "Renew";
                    cd.RegistrationDate = input.RegistrationDate;
                    cd.NextRenewalDate = input.NextRenewalDate;
                    cd.InvoiceNote = input.InvoiceNote;
                    cd.MapTo<ServiceRequestHistory>();
                    await _serviceRequestRepository.UpdateAsync((ServiceRequestHistory)cd);

                }
                if (input.checkadjustment == true)
                {
                    ServiceRequestHistory cd = new ServiceRequestHistory();
                    cd.Id = serviceRquestID;
                    cd.ServiceId = per.Id;
                    cd.Amount = input.Price;
                    cd.AdjustmentAmount = input.AdjustmentAmount;
                    cd.Comment = input.Comment;
                    cd.Actiontype = 3;
                    cd.ActionName = "Adjustment";
                    cd.InvoiceNote = input.InvoiceNote;
                    cd.Period = Convert.ToDateTime(input.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(input.NextRenewalDate).ToString("dd/MM/yyyy");
                    cd.MapTo<ServiceRequestHistory>();
                    await _serviceRequestRepository.InsertAndGetIdAsync((ServiceRequestHistory)cd);

                    invoicerequest Invreqobj = new invoicerequest();
                    Invreqobj.ServiceId = per.Id;
                    Invreqobj.ServiceReqId = cd.Id;
                    Invreqobj.TypeId = 1;
                    Invreqobj.Comment = input.InvoiceNote;
                    Invreqobj.Status = 0;
                    Invreqobj.Amount = input.AdjustmentAmount;
                    Invreqobj.InvoiceNote = input.InvoiceNote;
                    Invreqobj.Period = Convert.ToDateTime(input.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(input.NextRenewalDate).ToString("dd/MM/yyyy");
                    var InvId = Invreqobj.MapTo<invoicerequest>();
                    await _invoiceRequestRepository.InsertAsync((invoicerequest)Invreqobj);
                }





            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [AbpAuthorize(PermissionNames.Pages_SupportManageService)]
        public async Task DeleteService(int id)
        {
            await _manageserviceRepository.DeleteAsync(id);
        }


        //public ListResultDto<ListDataDto> GetDomainDate(GetServiceInput input)
        //{
        //    var monthAgo = DateTime.UtcNow.AddDays(30);

        //    var tasks = (from e1 in _manageserviceRepository.GetAll()

        //                 join servireq in _serviceRequestRepository.GetAll() on e1.Id equals servireq.ServiceId
        //                 into serviceRequest

        //                 join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
        //                  into distservice
        //                 from e2 in distservice.DefaultIfEmpty()
        //                 join e3 in _clientRepository.GetAll() on e1.ClientId equals e3.Id
        //                  into distclient
        //                 from e3 in distclient.DefaultIfEmpty()
        //                 join e4 in _userRepository.GetAll() on e1.EmployeeId equals e4.Id
        //                  into distuser
        //                 from e4 in distuser.DefaultIfEmpty()
        //                 join e5 in _serverRepository.GetAll() on e1.ServerType equals e5.Id
        //                  into distserver
        //                 from e5 in distserver.DefaultIfEmpty()
        //                 join e6 in _invoiceRequestRepository.GetAll()
        //                   on e1.Id equals e6.ServiceId
        //                   into invoicerequest
        //                 where (/*e1.ServiceId==1 && */e1.Cancelflag == false)
        //                 select new ListDataDto
        //                 {
        //                     Id = e1.Id,
        //                     DomainName = e1.DomainName,
        //                     Price = e1.Price,
        //                     HostingSpace = e1.HostingSpace,
        //                     ServiceId = e1.ServiceId,
        //                     ServiceName = e2.Name,
        //                     NextRenewalDate = e1.NextRenewalDate,
        //                     ClientId = e1.ClientId,
        //                     ClientName = e3.ClientName,
        //                     EmployeeId = e1.EmployeeId,
        //                     EmployeeName = e4.Name + " " + e4.Surname,
        //                     ServerName = e5.Name,
        //                     TypeName = e1.TypeName,
        //                     NoOfEmail = e1.NoOfEmail,
        //                     Typeofssl = e1.Typeofssl,
        //                     Title = e1.Title,
        //                     Cancelflag = e1.Cancelflag,
        //                     Actionstatus = serviceRequest.OrderByDescending(x => x.Id).Select(x => x.Actiontype).FirstOrDefault(),
        //                     Status = invoicerequest.OrderByDescending(x => x.Id).Select(x => x.Status).FirstOrDefault(),
        //                     Credits=e1.Credits
        //                     //StatusName = serviceRequest.OrderByDescending(x => x.Id).Select(x => x.ActionName).FirstOrDefault()
        //                 })
        //                .WhereIf(input.ServiceId.HasValue, x => x.ServiceId == input.ServiceId)
        //                //.WhereIf(tasks.ServiceId==2, x => x.NextRenewalDate == input.NextRenewalDate)
        //                .WhereIf(!input.DomainName.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(input.DomainName.ToLower()))
        //                .Where(p => p.NextRenewalDate >= DateTime.Today && p.NextRenewalDate <= monthAgo)

        //      .OrderByDescending(p => p.Id).ToList();
        //    return new ListResultDto<ListDataDto>(tasks.MapTo<List<ListDataDto>>());
        //}

        public ListResultDto<ListDataDto> GetHostingDate(GetServiceInput input)
        {
            var tenDaysAgo = DateTime.UtcNow.AddDays(10);
            var tasks = (from e1 in _manageserviceRepository.GetAll()
                         join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                         where (e1.ServiceId == 2 && e1.Cancelflag == false)
                         select new ListDataDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             ServiceId = e1.ServiceId,
                             NextRenewalDate = e1.NextRenewalDate,
                             Cancelflag = e1.Cancelflag

                         })
                        .WhereIf(input.ServiceId.HasValue, x => x.ServiceId == input.ServiceId)
                        .WhereIf(!input.DomainName.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(input.DomainName.ToLower()))
                        .Where(p => p.NextRenewalDate >= DateTime.Today && p.NextRenewalDate <= tenDaysAgo)
              //.WhereIf(input.DomainName != null && input.DomainName != "", x => x.DomainName == input.DomainName)
              .OrderByDescending(p => p.Id).ToList();
            return new ListResultDto<ListDataDto>(tasks.MapTo<List<ListDataDto>>());
        }

        public ListResultDto<ListDataDto> GetStorageDate(GetServiceInput input)
        {
            var tenDaysAgo = DateTime.UtcNow.AddDays(10);
            var tasks = (from e1 in _manageserviceRepository.GetAll()
                         join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                         where (e1.ServiceId == 3 && e1.Cancelflag == false)
                         select new ListDataDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             NextRenewalDate = e1.NextRenewalDate,
                             Cancelflag = e1.Cancelflag

                         })
                        .WhereIf(input.ServiceId.HasValue, x => x.ServiceId == input.ServiceId)
                        .WhereIf(!input.DomainName.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(input.DomainName.ToLower()))
                        .Where(p => p.NextRenewalDate >= DateTime.Today && p.NextRenewalDate <= tenDaysAgo)
              //.WhereIf(input.DomainName != null && input.DomainName != "", x => x.DomainName == input.DomainName)
              .OrderByDescending(p => p.Id).ToList();
            return new ListResultDto<ListDataDto>(tasks.MapTo<List<ListDataDto>>());
        }
        public ListResultDto<ListDataDto> GetemailDate(GetServiceInput input)
        {
            var tenDaysAgo = DateTime.UtcNow.AddDays(10);
            var tasks = (from e1 in _manageserviceRepository.GetAll()
                         join e2 in _serviceRepository.GetAll() on e1.ServiceId equals e2.Id
                         where (e1.ServiceId == 4 && e1.Cancelflag == false)
                         select new ListDataDto
                         {
                             Id = e1.Id,
                             DomainName = e1.DomainName,
                             NextRenewalDate = e1.NextRenewalDate,
                             Cancelflag = e1.Cancelflag

                         })
                        .WhereIf(input.ServiceId.HasValue, x => x.ServiceId == input.ServiceId)
                        .WhereIf(!input.DomainName.IsNullOrEmpty(), p => p.DomainName.ToLower().Contains(input.DomainName.ToLower()))
                        .Where(p => p.NextRenewalDate >= DateTime.Today && p.NextRenewalDate <= tenDaysAgo)
              .OrderByDescending(p => p.Id).ToList();
            return new ListResultDto<ListDataDto>(tasks.MapTo<List<ListDataDto>>());
        }



        public async Task UpdateDashboardService(int id)
        {
            try
            {
                var per = await _manageserviceRepository.FirstOrDefaultAsync(id);

                per.Cancelflag = true;
                per.CancelDate = DateTime.Now;
                //per.RenewalDate = DateTime.Now;

                await _manageserviceRepository.UpdateAsync(per);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public async Task DashboardMarkAsConfirm(UpdateDataDto input)
        {
            if (input.ServiceId == 1)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.TypeName = null;
                input.NoOfEmail = null;
                input.ServerType = null;
                input.Title = null;
                input.Typeofssl = null;

            }
            if (input.ServiceId == 2 || input.ServiceId == 3)
            {

                input.TypeName = null;
                input.NoOfEmail = null;
            }
            if (input.ServiceId == 8)
            {
                input.TypeName = null;
                input.NoOfEmail = null;
                input.HostingSpace = null;
                input.Typeofssl = null;
            }

            if (input.ServiceId == 4)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
            }
            if (input.ServiceId == 5)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.TypeName = null;
                input.NoOfEmail = null;
                input.ServerType = null;
                input.Title = null;

            }
            if (input.ServiceId == 6)
            {
                input.HostingSpace = null;
                input.DatabaseSpace = null;
                input.TypeName = null;
                input.NoOfEmail = null;
                input.ServerType = null;
                input.Typeofssl = null;

            }
            var per = await _manageserviceRepository.FirstOrDefaultAsync(input.Id);

            per.Price = input.Price;
            per.Comment = input.Comment;
            per.HostingSpace = input.HostingSpace;
            per.DatabaseSpace = input.DatabaseSpace;
            per.NoOfEmail = input.NoOfEmail;
            per.Term = input.Term;
            //per.NextRenewalDate = input.NextRenewalDate;
            per.RenewalDate = input.NextRenewalDate;
            //per.ClientId = input.ClientId;
            await _manageserviceRepository.UpdateAsync(per);
            //var serviceRquestID1 = _serviceRequestRepository.GetAll().Where(e => e.Id == per.ServiceId).Select(e => e.Id).FirstOrDefault();
            try
            {

                ServiceRequestHistory cd = new ServiceRequestHistory();
                //cd.Id = serviceRquestID;
                cd.ServiceId = per.Id;
                cd.Amount = per.Price;
                cd.AdjustmentAmount = 0;
                cd.Comment = per.Comment;
                cd.Actiontype = 4;
                cd.RegistrationDate = per.RegistrationDate;
                //cd.NextRenewalDate = per.NextRenewalDate;
                cd.ActionName = "Mark As Confirm";
                cd.InvoiceNote = input.InvoiceNote;
                cd.Period = Convert.ToDateTime(input.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(input.NextRenewalDate).ToString("dd/MM/yyyy");
                var ServiceReqId = cd.MapTo<ServiceRequestHistory>();
                await _serviceRequestRepository.InsertAndGetIdAsync((ServiceRequestHistory)cd);


                //Insert into InvoiceRequest table
                invoicerequest Invreqobj = new invoicerequest();
                Invreqobj.ServiceId = per.Id;
                Invreqobj.ServiceReqId = ServiceReqId.Id;
                Invreqobj.TypeId = 1;
                Invreqobj.Comment = input.InvoiceNote;
                Invreqobj.Status = 0;
                Invreqobj.Amount = per.Price;
                Invreqobj.InvoiceNote = input.InvoiceNote;
                Invreqobj.Period = Convert.ToDateTime(input.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(input.NextRenewalDate).ToString("dd/MM/yyyy");
                var InvId = Invreqobj.MapTo<invoicerequest>();
                await _invoiceRequestRepository.InsertAsync((invoicerequest)Invreqobj);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public ListResultDto<ListDataDto> GetClientNameActive(GetServiceInput input)
        {

            var persons = (from e1 in _clientRepository.GetAll()
                           join e2 in _manageserviceRepository.GetAll() on e1.Id equals e2.ClientId

                           //where (e1.ClientId!= null)
                           select new ListDataDto
                           {
                               ClientId = e1.Id,
                               ClientName = e1.ClientName
                           }).Distinct()
                           .OrderBy(x => x.ClientName).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }

        public ListResultDto<ListDataDto> GetEmployeeNameActive(GetServiceInput input)
        {

            var persons = (from e1 in _userRepository.GetAll()
                           join e2 in _manageserviceRepository.GetAll() on e1.Id equals e2.EmployeeId

                           //where (e1.ClientId!= null)
                           select new ListDataDto
                           {
                               EmployeeId = (int)e1.Id,
                               EmployeeName = e1.Name + " " + e1.Surname,

                           }).Distinct()
                           .OrderBy(x => x.EmployeeName).ToList();

            return new ListResultDto<ListDataDto>(persons.MapTo<List<ListDataDto>>());


        }

        public ListResultDto<HistoryListDto> GetAllServiceHistory(int id)
        {
            var result = (from c in _manageserviceRepository.GetAll()
                          join a in _serviceRequestRepository.GetAll()
                          on c.Id equals a.ServiceId
                          join b in _serviceRepository.GetAll()
                          on c.ServiceId equals b.Id
                          where (c.Id == id && a.Actiontype != 4)
                          select new HistoryListDto
                          {
                              Id = a.Id,
                              ServiceId = a.ServiceId,
                              ServiceName = b.Name,
                              Amount = a.Amount,
                              AdjustmentAmount = a.AdjustmentAmount,
                              Comment = a.Comment,
                              Actiontype = a.Actiontype,
                              ActionName = a.ActionName,
                              RegistrationDate = a.RegistrationDate,
                              NextRenewalDate = a.NextRenewalDate,
                              CreatedOn = a.CreationTime,


                          }).OrderByDescending(x => x.Id).ToList();
            return new ListResultDto<HistoryListDto>(result.MapTo<List<HistoryListDto>>());
        }

        public List<ListDto> GetUserName()
        {
            var persons = (from a in _userRepository.GetAll()
                           join b in _userRoleRepository.GetAll()
                           on a.Id equals b.UserId
                           join c in _roleRepository.GetAll()
                           on b.RoleId equals c.Id
                           where a.IsActive == true && (c.Name.ToLower() == "admin" || c.Name.ToLower() == "support" || c.Name.ToLower() == "marketing leader")
                           select new ListDto
                           {
                               EmployeeId = (int)a.Id,
                               EmployeeName = a.Name + " " + a.Surname
                           }).Distinct()
                            .OrderBy(x => x.EmployeeName).ToList();

            return persons;
        }

        public List<ListDto> GetActiveUserName()
        {
            var persons = (from a in _userRepository.GetAll()
                           join e2 in _manageserviceRepository.GetAll()
                           on a.Id equals e2.EmployeeId
                           join b in _userRoleRepository.GetAll()
                           on a.Id equals b.UserId
                           join c in _roleRepository.GetAll()
                           on b.RoleId equals c.Id

                           where a.IsActive == true && (c.Name.ToLower() == "admin" || c.Name.ToLower() == "support" || c.Name.ToLower() == "marketing leader")
                           select new ListDto
                           {
                               EmployeeId = (int)a.Id,
                               EmployeeName = a.Name + " " + a.Surname
                           }).Distinct()
                          .OrderBy(x => x.EmployeeName).ToList();

            return persons;

        }

        public List<ListDto> GetUserNameById(int id)
        {
            var persons = (from a in _userRepository.GetAll()
                           join b in _userRoleRepository.GetAll()
                           on a.Id equals b.UserId
                           join c in _roleRepository.GetAll()
                           on b.RoleId equals c.Id
                           where a.IsActive == true && (c.Name.ToLower() == "admin" || c.Name.ToLower() == "support" || c.Name.ToLower() == "marketing leader") &&
                                    a.Id != id
                           select new ListDto
                           {
                               EmployeeId = (int)a.Id,
                               EmployeeName = a.Name + " " + a.Surname
                           })
                             .OrderBy(x => x.EmployeeName).ToList();

            return persons;
        }

        public ListResultDto<ListInvoiceRequestDto> GetInvoiceRequestService(int sid)
        {

            var tasks = (from e1 in _manageserviceRepository.GetAll()
                         join e2 in _invoiceRequestRepository.GetAll() on e1.Id equals e2.ServiceId
                         where (e2.ServiceId == sid)
                         select new ListInvoiceRequestDto
                         {
                             Id = e2.Id,
                             Amount = e2.Amount,
                             Comment = e2.Comment,
                             CreationTime = e2.CreationTime,
                             Status = e2.Status,
                             DomainName = e1.DomainName,
                             ServiceId = e2.ServiceId,
                             InvoiceNote = e2.InvoiceNote,
                         }).OrderByDescending(x => x.Id)
                     .ToList();
            return new ListResultDto<ListInvoiceRequestDto>(tasks.MapTo<List<ListInvoiceRequestDto>>());

            //return tasks;
        }

        public async Task CreateInvoiceRequestService(InvoiceRequestDto input)
        {
            var Userlist = (from u in _userRepository.GetAll()
                            join ur in _userRoleRepository.GetAll()
                            on u.Id equals ur.UserId
                            join r in _roleRepository.GetAll()
                            on ur.RoleId equals r.Id
                            where (r.Name == "Accounts" || r.Name == "Admin") && u.IsActive == true
                            select u).ToList();

            ServiceRequestHistory cd = new ServiceRequestHistory();
            var result = _manageserviceRepository.GetAll().Where(x=>x.Id==input.Id).Select(x=>x).FirstOrDefault();
            cd.ServiceId = input.Id;
            cd.Amount = input.Amount;
            cd.AdjustmentAmount = 0;
            cd.Comment = input.Comment;
            cd.Actiontype = 5;
            cd.Period = Convert.ToDateTime(result.RegistrationDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(result.NextRenewalDate).ToString("dd/MM/yyyy");
            cd.RegistrationDate = null;
            cd.NextRenewalDate = null;
            cd.ActionName = "Manual Invoice";
            cd.InvoiceNote = input.InvoiceNote;

            var ServiceReqId = cd.MapTo<ServiceRequestHistory>();
            await _serviceRequestRepository.InsertAndGetIdAsync((ServiceRequestHistory)cd);

            invoicerequest Invreqobj = new invoicerequest();
            Invreqobj.TypeId = 1;
            Invreqobj.Comment = input.InvoiceNote;
            Invreqobj.Status = 0; // 0=pending,1=approved
            Invreqobj.Amount = input.Amount;
            Invreqobj.ServiceId = cd.ServiceId;
            Invreqobj.ServiceReqId = cd.Id;
            Invreqobj.InvoiceNote = input.InvoiceNote;
            Invreqobj.Period = cd.Period;
            var InvId = Invreqobj.MapTo<invoicerequest>();
            await _invoiceRequestRepository.InsertAsync((invoicerequest)Invreqobj);

            //get Domain Name
            var domainname = _manageserviceRepository.GetAll().Where(x => x.Id == input.Id).Select(x => x.DomainName).FirstOrDefault();
            User[] users = Userlist.ToArray();   //all accountant and admin users

            #region Push Notification
            string message = "New invoice request generated for domain " + domainname;
            string severity = "success";

            await _appNotifier.SendInvoiceRequestMessageAsync(
                users,
                message,
                severity.ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
                );
            #endregion

            #region sendmail
            GetInvoiceRequestDto item = new GetInvoiceRequestDto();
            //item.FromEmail = _userRepository.GetAll().Where(x => x.Id == userId).Select(x => x.EmailAddress).FirstOrDefault();
            item.DomainName = domainname;
            item.EmailTitle = "New Invoice Request is generated for Domain " + item.DomainName;
            item.Amount = input.Amount;
            item.Comment = input.Comment;
            foreach (var email in Userlist)
            {
                item.toEmail = email.EmailAddress;
                await _userEmailer.SendEmailForNewInvoiceRequest(item);
            }
            #endregion
        }

        public List<DomainListDto> GetDomainNameList(string domainname)
        {
            var result = (from a in _manageserviceRepository.GetAll()
                          where a.DomainName.Contains((domainname))
                          select new DomainListDto
                          {
                              //Id = (int)a.Id,
                              DomainName = a.DomainName
                          })
                          .OrderBy(x => x.DomainName).Distinct().ToList();
            return result;

        }

        public async Task UpdateServiceWithoutClient(UpdateServiceDto input)
        {
               
                var per = await _manageserviceRepository.FirstOrDefaultAsync(input.Id);
                per.DomainName = input.DomainName;
                per.ServiceId = input.ServiceId;
                per.ClientId = input.ClientId.HasValue ? input.ClientId.Value : 0;
                await _manageserviceRepository.UpdateAsync(per);
            }
           
    }


}
