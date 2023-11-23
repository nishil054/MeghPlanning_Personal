using System;
using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using AbpProjects.Authorization;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.EntityFramework;
using AbpProjects.MeghPlanningNotification;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.CallCategories;
using AbpProjects.Opportunities;

namespace AbpProjects.Migrations.SeedData
{   
    public class TenantRoleAndUserBuilder
    {
        private readonly AbpProjectsDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(AbpProjectsDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
            CreateDefaultServiceName();
            CreateDefaultServerType();
            CreateDefaultNotification();
            CreateDefaultLeaveType();
            CreateDefaultLeaveStatus();
            CreateFollowUpType();
            CreateCallCategory();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin)
                {
                    IsStatic = true
                };

                adminRole.SetNormalizedName();

                _context.Roles.Add(adminRole);
                _context.SaveChanges();

                //Grant all permissions to admin role
                var permissions = PermissionFinder
                    .GetAllPermissions(new AbpProjectsAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                    .ToList();
                //var permissions = PermissionFinder
                //    .GetAllPermissions(new AbpProjectsAuthorizationProvider())
                //    .Where(p => p.Description.ToString()=="admin")
                //    .ToList();
                foreach (var permission in permissions)
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            TenantId = _tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        });
                }

                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com", User.DefaultPassword);
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
            //Supervisor Role
            var SuperRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.Supervisor);
            if (SuperRole == null)
            {
                SuperRole = new Role(_tenantId, StaticRoleNames.Tenants.Supervisor, StaticRoleNames.Tenants.Supervisor)
                {
                    IsStatic = true
                };

                SuperRole.SetNormalizedName();

                _context.Roles.Add(SuperRole);
                _context.SaveChanges();
            }
            //Employee Role
            var EmpRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.Employee);
            if (EmpRole == null)
            {
                EmpRole = new Role(_tenantId, StaticRoleNames.Tenants.Employee, StaticRoleNames.Tenants.Employee)
                {
                    IsStatic = true
                };

                EmpRole.SetNormalizedName();

                _context.Roles.Add(EmpRole);
                _context.SaveChanges();
            }
            //support role
            var supportRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.Support);
            if (supportRole == null)
            {
                supportRole = new Role(_tenantId, StaticRoleNames.Tenants.Support, StaticRoleNames.Tenants.Support)
                {
                    IsStatic = true
                };

                supportRole.SetNormalizedName();

                _context.Roles.Add(supportRole);
                _context.SaveChanges();
            }
            //Marketing Leader
            var MarketRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.Marketing_Leader);
            if (MarketRole == null)
            {
                MarketRole = new Role(_tenantId, StaticRoleNames.Tenants.Marketing_Leader, StaticRoleNames.Tenants.Marketing_Leader)
                {
                    IsStatic = true
                };

                MarketRole.SetNormalizedName();

                _context.Roles.Add(MarketRole);
                _context.SaveChanges();
            }
            //HR
            var HRRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.HR);
            if (HRRole == null)
            {
                HRRole = new Role(_tenantId, StaticRoleNames.Tenants.HR, StaticRoleNames.Tenants.HR)
                {
                    IsStatic = true
                };

                HRRole.SetNormalizedName();

                _context.Roles.Add(HRRole);
                _context.SaveChanges();
            }
            //Support
            var SupportRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.Support);
            if (SupportRole == null)
            {
                SupportRole = new Role(_tenantId, StaticRoleNames.Tenants.Support, StaticRoleNames.Tenants.Support)
                {
                    IsStatic = true
                };

                SupportRole.SetNormalizedName();

                _context.Roles.Add(SupportRole);
                _context.SaveChanges();
            }
            //operating Leader
            var operatingRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.OpporatingLeader);
            if (operatingRole == null)
            {
                operatingRole = new Role(_tenantId, StaticRoleNames.Tenants.OpporatingLeader, StaticRoleNames.Tenants.OpporatingLeader)
                {
                    IsStatic = true
                };
                operatingRole.SetNormalizedName();
                _context.Roles.Add(operatingRole);
                _context.SaveChanges();
            }

            //telemarketing leader
            var telemarketingRole = _context.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.Telemarketing);
            if (telemarketingRole == null)
            {
                telemarketingRole = new Role(_tenantId, StaticRoleNames.Tenants.Telemarketing, StaticRoleNames.Tenants.Telemarketing)
                {
                    IsStatic = true
                };
                telemarketingRole.SetNormalizedName();
                _context.Roles.Add(telemarketingRole);
                _context.SaveChanges();
            }
        }

        private void CreateDefaultServerType()
        {
            var Serverlinux = _context.ServerTypeDetails.FirstOrDefault(p => p.Name == "Linux");
            if (Serverlinux == null)
            {
                _context.ServerTypeDetails.Add(
                    new ServerTypeDetail
                    {

                        Name = "Linux"

                    });
            }
            var Serverwindow = _context.ServerTypeDetails.FirstOrDefault(p => p.Name == "Window");
            if (Serverwindow == null)
            {
                _context.ServerTypeDetails.Add(
                     new ServerTypeDetail
                     {
                         Name = "Window"
                     });
            }


        }

        private void CreateDefaultServiceName()
        {
            var servicedomain = _context.Service.FirstOrDefault(p => p.Name == "Domain");
            if (servicedomain == null)
            {
                _context.Service.Add(
                    new Service
                    {

                        Name = "Domain"

                    });
            }
            var Servicehosting = _context.Service.FirstOrDefault(p => p.Name == "Hosting");
            if (Servicehosting == null)
            {
                _context.Service.Add(
                     new Service
                     {
                         Name = "Hosting"
                     });
            }
            var Servicestorage = _context.Service.FirstOrDefault(p => p.Name == "Storage");
            if (Servicestorage == null)
            {
                _context.Service.Add(
                    new Service
                    {
                        Name = "Storage"
                    });
            }
            var Serviceemail = _context.Service.FirstOrDefault(p => p.Name == "Email");
            if (Serviceemail == null)
            {
                _context.Service.Add(
                    new Service
                    {
                        Name = "Email"
                    });
            }

            var Servicessl = _context.Service.FirstOrDefault(p => p.Name == "SSL");
            if (Servicessl == null)
            {
                _context.Service.Add(
                     new Service
                     {
                         Name = "SSL"
                     });
            }

            var ServiceMaintenance = _context.Service.FirstOrDefault(p => p.Name == "Maintenance");
            if (ServiceMaintenance == null)
            {
                _context.Service.Add(
                     new Service
                     {
                         Name = "Maintenance"
                     });
            }

            var SendGrid = _context.Service.FirstOrDefault(p => p.Name == "Send Grid");
            if (SendGrid == null)
            {
                _context.Service.Add(
                     new Service
                     {
                         Name = "Send Grid"
                     });
            }

            var DataBase = _context.Service.FirstOrDefault(p => p.Name == "Database");
            if (DataBase == null)
            {
                _context.Service.Add(
                     new Service
                     {
                         Name = "Database"
                     });
            }
 
        }

        private void CreateDefaultNotification()
        {

            var notification = _context.Notifications.FirstOrDefault(p => p.Title == "Reminder");
            if (notification == null)
            {
                _context.Notifications.Add(
                    new Notification
                    {

                        Title = "Reminder",
                        Id = 1

                    });
            }
        }

        private void CreateDefaultLeaveType()
        {
            var leavetype = _context.Leavetypes.FirstOrDefault(p => p.Type == "Half Day - First");
            if (leavetype == null)
            {
                _context.Leavetypes.Add(
                    new LeaveType.Leavetype
                    {
                        Id = 1,
                        Type = "Half Day - First"
                    });
            }
            var leavetype2 = _context.Leavetypes.FirstOrDefault(p => p.Type == "Half Day - Second");
            if (leavetype2 == null)
            {
                _context.Leavetypes.Add(
                    new LeaveType.Leavetype
                    {
                        Id = 2,
                        Type = "Half Day - Second"
                    });
            }
            var leavetype3 = _context.Leavetypes.FirstOrDefault(p => p.Type == "Full Day");
            if (leavetype3 == null)
            {
                _context.Leavetypes.Add(
                    new LeaveType.Leavetype
                    {
                        Id = 3,
                        Type = "Full Day"
                    });
            }
            var leavetype4 = _context.Leavetypes.FirstOrDefault(p => p.Type == "Short Leave");
            if (leavetype4 == null)
            {
                _context.Leavetypes.Add(
                    new LeaveType.Leavetype
                    {
                        Id = 4,
                        Type = "Short Leave"
                    });
            }
        }

        private void CreateDefaultLeaveStatus()
        {
            var leavestatus = _context.Leavestatuses.FirstOrDefault(p => p.Status == "Pending");
            if (leavestatus == null)
            {
                _context.Leavestatuses.Add(
                    new LeaveStatus.Leavestatus
                    {
                        Id = 0,
                        Status = "Pending"
                    });
            }
            var leavestatus2 = _context.Leavestatuses.FirstOrDefault(p => p.Status == "Approve");
            if (leavestatus2 == null)
            {
                _context.Leavestatuses.Add(
                    new LeaveStatus.Leavestatus
                    {
                        Id = 1,
                        Status = "Approve"
                    });
            }
            var leavestatus3 = _context.Leavestatuses.FirstOrDefault(p => p.Status == "Reject");
            if (leavestatus3 == null)
            {
                _context.Leavestatuses.Add(
                    new LeaveStatus.Leavestatus
                    {
                        Id = 2,
                        Status = "Reject"
                    });
            }
            var leavestatus4 = _context.Leavestatuses.FirstOrDefault(p => p.Status == "Leave Cancel Request");
            if (leavestatus4 == null)
            {
                _context.Leavestatuses.Add(
                    new LeaveStatus.Leavestatus
                    {
                        Id = 3,
                        Status = "Leave Cancel Request"
                    });
            }
            var leavestatus5 = _context.Leavestatuses.FirstOrDefault(p => p.Status == "Cancel Request Approve");
            if (leavestatus5 == null)
            {
                _context.Leavestatuses.Add(
                    new LeaveStatus.Leavestatus
                    {
                        Id = 4,
                        Status = "Cancel Request Approve"
                    });
            }
            var leavestatus6 = _context.Leavestatuses.FirstOrDefault(p => p.Status == "Cancelled");
            if (leavestatus6 == null)
            {
                _context.Leavestatuses.Add(
                    new LeaveStatus.Leavestatus
                    {
                        Id = 5,
                        Status = "Cancelled"
                    });
            }
        }

        private void CreateCallCategory()
        {
            var leavestatus = _context.CallCategories.FirstOrDefault(p => p.Name == "Dead");
            if (leavestatus == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 1,
                        Name = "Dead"
                    });
            }
            var leavestatus2 = _context.CallCategories.FirstOrDefault(p => p.Name == "Cold");
            if (leavestatus2 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 2,
                        Name = "Cold"
                    });
            }
            var leavestatus3 = _context.CallCategories.FirstOrDefault(p => p.Name == "Warm");
            if (leavestatus3 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 3,
                        Name = "Warm"
                    });
            }
            var leavestatus4 = _context.CallCategories.FirstOrDefault(p => p.Name == "Hot");
            if (leavestatus4 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 4,
                        Name = "Hot"
                    });
            }
            var leavestatus5 = _context.CallCategories.FirstOrDefault(p => p.Name == "Lost");
            if (leavestatus5 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 5,
                        Name = "Lost"
                    });
            }
            var leavestatus6 = _context.CallCategories.FirstOrDefault(p => p.Name == "Closed");
            if (leavestatus6 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 6,
                        Name = "Closed"
                    });
            }
            var leavestatus7 = _context.CallCategories.FirstOrDefault(p => p.Name == "Opportunity");
            if (leavestatus7 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 7,
                        Name = "Opportunity"
                    });
            }
            var leavestatus8 = _context.CallCategories.FirstOrDefault(p => p.Name == "Opportunity Dead");
            if (leavestatus8 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 8,
                        Name = "Opportunity Dead"
                    });
            }
            var leavestatus9 = _context.CallCategories.FirstOrDefault(p => p.Name == "Inquiry");
            if (leavestatus9== null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 9,
                        Name = "Inquiry"
                    });
            }
            var leavestatus10 = _context.CallCategories.FirstOrDefault(p => p.Name == "Tele Caller");
            if (leavestatus10 == null)
            {
                _context.CallCategories.Add(
                    new CallCategories.CallCategory
                    {
                        Id = 6,
                        Name = "Tele Caller"
                    });
            }
        }
        public void CreateFollowUpType()
        {
            var type1 = _context.Followuptypes.FirstOrDefault(p => p.FollowUpType == "Phone");
            if (type1 == null)
            {
                _context.Followuptypes.Add(
                    new Followuptype
                    {
                        FollowUpType = "Phone"

                    });
            }
            var type2 = _context.Followuptypes.FirstOrDefault(p => p.FollowUpType == "Online");
            if (type2 == null)
            {
                _context.Followuptypes.Add(
                    new Followuptype
                    {
                        FollowUpType = "Online"

                    });
            }
            var type3 = _context.Followuptypes.FirstOrDefault(p => p.FollowUpType == "Meeting");
            if (type3 == null)
            {
                _context.Followuptypes.Add(
                    new Followuptype
                    {
                        FollowUpType = "Meeting"

                    });
            }
            
        }
    }
}