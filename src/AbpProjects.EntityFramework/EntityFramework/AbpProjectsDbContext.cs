using System.Data.Common;
using System.Data.Entity;
using Abp.DynamicEntityProperties;
using Abp.Zero.EntityFramework;
using AbpProjects.Authorization.Roles;
using AbpProjects.Authorization.Users;
using AbpProjects.Company;
using AbpProjects.Document;
using AbpProjects.Holidays;
using AbpProjects.MeghPlanningSupports;
using AbpProjects.ImportExcelData;
using AbpProjects.MultiTenancy;
using AbpProjects.Project;
using AbpProjects.ProjectType;
using AbpProjects.Team;
using AbpProjects.VPS;
using AbpProjects.TimeSheet;
using AbpProjects.UserLoginDetails;
//using AbpProjects.UserLogin;
using AbpProjects.WorkType;
using AbpProjects.ManageKnowledgeCenter;
using AbpProjects.Category;
using AbpProjects.InvoiceRequest;

using AbpProjects.Bills;
using AbpProjects.PerformaInvoices;
using AbpProjects.Productions;
using AbpProjects.Receipt;
using AbpProjects.ProjectMilestone;
using AbpProjects.FinancialYear;
using AbpProjects.gstdashboard;
using AbpProjects.Clientsaddress;
using AbpProjects.ExpenseCategories;
using AbpProjects.ExpenseEnteryForm;
using AbpProjects.NthLevelCategory;
using AbpProjects.BulkData;
using AbpProjects.MeghPlanningNotification;
using AbpProjects.FileUploadByDirective;
using AbpProjects.LeaveType;
using AbpProjects.LeaveStatus;
using AbpProjects.LeaveApplication;
using AbpProjects.CallCategories;
using AbpProjects.Opportunities;

namespace AbpProjects.EntityFramework
{
    public class AbpProjectsDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        public virtual IDbSet<Service> Service { get; set; }
        public virtual IDbSet<ServiceRequestHistory> ServiceRequestHistorys { get; set; }
        public virtual IDbSet<Typename> Typenames { get; set; }
        public virtual IDbSet<ServerTypeDetail> ServerTypeDetails { get; set; }
        public virtual IDbSet<ManageService> ManageServices { get; set; }
        public virtual IDbSet<invoicerequest> Invoicerequests { get; set; }
        public virtual IDbSet<Client> Clients { get; set; }
        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public virtual IDbSet<BillPymtRecd> BillPymtRecd { get; set; }
        public virtual IDbSet<Production> Production { get; set; }

        public virtual IDbSet<Bill> Bills { get; set; }
        public virtual IDbSet<PerformaInvoice> PerformaInvoice { get; set; }
        public virtual IDbSet<company> Companies { get; set; }
        public virtual IDbSet<team> Teams { get; set; }
        public virtual IDbSet<projecttype> Projecttypes { get; set; }
        public virtual IDbSet<worktype> Worktypes { get; set; }
        public virtual IDbSet<project> Projects { get; set; }
        public virtual IDbSet<ImportUserStoryDetails> ImportUserStoryDetails { get; set; }
        public virtual IDbSet<ProjectStatus> ProjectStatuses { get; set; }
        public virtual IDbSet<document> Documents { get; set; }
        public virtual IDbSet<vps> Vpss { get; set; }
        public virtual IDbSet<Holiday> Holidays { get; set; }
        public virtual IDbSet<Projecttype_details> Projecttype_details { get; set; }

        public virtual IDbSet<timesheet> Timesheets { get; set; }
        public virtual IDbSet<UserLoginData> UserLogins { get; set; }
        public virtual IDbSet<ImportFTPDetails> ImportFTPDetails { get; set; }
        public virtual IDbSet<manageKnowledgeCenter> ManageKnowledgeCenters { get; set; }
        public virtual IDbSet<KnowledgeDocuments> KnowledgeDocuments { get; set; }
        public virtual IDbSet<category> Category { get; set; }
        public virtual IDbSet<projectMilestone> ProjectMilestones { get; set; }
        public virtual IDbSet<financialYear> FinancialYears { get; set; }
        public virtual IDbSet<gstDashboard> GstDashboards  { get; set; }
        public virtual IDbSet<ClientAddress> ClientAddress { get; set; }
        public virtual IDbSet<ExpenseCategory> ExpenseCategorys { get; set; }
        public virtual IDbSet<ExpenseSubcategory> ExpenseSubcategorys { get; set; }
        public virtual IDbSet<tbl_category_team> Tbl_Category_Teams { get; set; }
        public virtual IDbSet<ExpenseEntry> ExpenseEntries { get; set; }
        public virtual IDbSet<CategoryMaster> CategoryMasters { get; set; }
        public virtual IDbSet<bulkmaster> Bulkmasters { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }
        public virtual IDbSet<UserNotification> UserNotifications { get; set; }

        public virtual IDbSet<Documentmaster> Documentmasters { get; set; }
        public virtual IDbSet<Documentchild> Documentchildren { get; set; }

        public virtual IDbSet<Leavetype> Leavetypes { get; set; }
        public virtual IDbSet<Leavestatus> Leavestatuses { get; set; }
        public virtual IDbSet<Leaveapplication> Leaveapplications { get; set; }
        public virtual IDbSet<CallCategory> CallCategories { get; set; }
        public virtual IDbSet<Opportunity> Opportunities { get; set; }
        public virtual IDbSet<InterestedOpportunity> InterestedOpportunities { get; set; }
        public virtual IDbSet<OpportunityFollowUp> OpportunityFollowUps { get; set; }
        public virtual IDbSet<FollowupIntrest> FollowupIntrests { get; set; }
        public virtual IDbSet<Followuptype> Followuptypes { get; set; }

        public AbpProjectsDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in AbpProjectsDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of AbpProjectsDbContext since ABP automatically handles it.
         */
        public AbpProjectsDbContext(string nameOrConnectionString)
            :  base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public AbpProjectsDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public AbpProjectsDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<Bill>().Property(x => x.SnachCount).HasPrecision(16, 3);
            //modelBuilder.Entity<DynamicProperty>().Property(p => p.PropertyName).HasPrecision(18, 2);

            modelBuilder.Entity<DynamicProperty>().Property(p => p.PropertyName).HasMaxLength(250);
            modelBuilder.Entity<DynamicEntityProperty>().Property(p => p.EntityFullName).HasMaxLength(250);
        }
    }
}
