
(function () {
    var controllerId = 'app.views.layout.sidebarNav';
    angular.module('app').controller(controllerId, [
        '$rootScope', '$state', 'appSession', '$location',
        function ($rootScope, $state, appSession, weblocation) {
            var vm = this;

            vm.menuItems = [
                createMenuItem(App.localize("DashBoard"), "", "dashboard", "home"),
                createMenuItem(App.localize("Employee"), "Pages.Users", "people", "users"),
                createMenuItem(App.localize("Roles"), "Pages.Roles", "local_offer", "roles"),


                createMenuItem(App.localize("DataVault"), "Pages.DataVault", "display_settings", "", [
                    createMenuItem(App.localize("Tenants"), "Pages.Tenants", "dashboard", "tenants"),
                    createMenuItem(App.localize("Company"), "Pages.DataVault.Company", "info", "company"),
                    createMenuItem("Holiday", "Pages.DataVault.Admin.Holiday", "info", "holiday"),
                    createMenuItem(App.localize("ProjectType"), "Pages.DataVault.ProjectType", "info", "projecttype"),
                    createMenuItem(App.localize("WorkType"), "Pages.DataVault.WorkType", "info", "worktype"),

                    //createMenuItem("Holiday", "Pages.Admin_Holiday", "info", "holiday"),
                    createMenuItem("Document", "Pages.DataVault.Admin.Documents", "info", "document"),
                    createMenuItem(App.localize("TypeName"), "Pages.DataVault.TypeName", "info", "typename"),
                    createMenuItem("Category", "Pages.DataVault.Category", "description", "category"),
                    createMenuItem("Leaves", "Pages.DataVault.Admin.Leaves", "description", "manageleaves"),
                    createMenuItem("Financial Year", "Pages.DataVault.Financialyear", "description", "financialyear")
                ]),
                createMenuItem(App.localize("Page_Expense"), "Pages.Expense", "request_quote", "expcategorylist", [
                    createMenuItem("Expense Category", "Pages.Expense.Category", "inventory", "expcategorylist"),
                    createMenuItem("Expense Heads", "Pages.Expense.SubCategory", "inventory", "expensesubcategorylist"),
                    createMenuItem("Expense Entry", "Pages.Expense.Entry", "inventory", "expenseentrylist")
                ]),
                createMenuItem("FTP Details", "Pages.Import", "lan", "importftp"),
                createMenuItem("GST Dashboard", "Pages.GSTDashboard", "inventory", "gstdashboard"),
                createMenuItem("Auditlog", "Pages.AuditLogs", "inventory", "auditlogs"),
                /*createMenuItem("UserStory Details", "Pages.UserStory", "contact_page", "importuserstory"),*/
                /*createMenuItem("UserStory Reports", "Pages.UserStoryReport", "contact_page", "userStory"),*/
                createMenuItem("Manage Category", "Pages.ManageCategory", "description", "nthlevelcategory"),


                createMenuItem("VPS", "Pages.Employee.VPS", "description", "vpslist"),
                createMenuItem("VPS", "Pages.Support.VPS", "cloud", "vps"),
                /*createMenuItem(App.localize("ManageServices"), "Pages.SupportManageService", "settings", "supportpages"),*/
                createMenuItem(App.localize("ManageServices"), "Pages.Support", "settings", "", [
                    createMenuItem(App.localize("ManageServices"), "Pages.Support", "settings", "supportpages"),
                    createMenuItem("Service Renewal", "Pages.SupportManageService", "dns", "servicerenewal"),
                ]),
                //createMenuItem(App.localize("Projects"), "Pages.Project", "dns", "project"),


                createMenuItem(App.localize("Opportunity"), "Pages.Opportunity", "local_offer", "", [
                    createMenuItem(App.localize("Opportunity"), "Pages.Opportunity", "local_offer", "inquiry"),
                    createMenuItem(App.localize("GeneralOpportunity"), "Pages.GeneralOpportunity", "local_offer", "opportunity"),
                    createMenuItem(App.localize("MyOpportunity"), "Pages.MyOpportunity", "local_offer", "myopportunity")

                ]),

                createMenuItem(App.localize("Projects"), "Pages.Project", "dns", "activeprojects", [
                    //createMenuItem("Projects", "Pages.Project", "dns", "project"),
                    createMenuItem("All Projects", "Pages.AllProjects", "dns", "allprojects"),
                    createMenuItem("Active Projects", "Pages.ActiveProjects", "dns", "activeprojects"),
                    createMenuItem("AMC Projects", "Pages.AMCProjects", "dns", "amcprojects"),
                    createMenuItem("On Going Projects", "Pages.OnGoingProjects", "dns", "ongoingprojects"),
                    createMenuItem("Invoice / Collection Projects", "Pages.InvoiceCollectionProjects", "dns", "invoicecollectionprojects"),
                    createMenuItem("Completed Projects", "Pages.CompletedProjects", "dns", "completedprojects"),
                    createMenuItem("On Hold Projects", "Pages.OnHoldProjects", "dns", "onholdprojects"),
                    createMenuItem(App.localize("ProjectsWithoutClient"), "Pages.ProjectsWithoutClient", "dns", "projectswithoutclient")
                ]),

                createMenuItem(App.localize("Clients"), "Pages.Clients", "account_box", "clients"),
                createMenuItem(App.localize("TimeSheet"), "Pages.TimeSheet", "text_snippet", "timesheet"),

                createMenuItem(App.localize("LeaveApplication"), "Pages.LeaveApplication", "text_snippet", "leaveapplication", [
                    createMenuItem(App.localize("LeaveApplication"), "Pages.LeaveApplication", "text_snippet", "leaveapplication"),
                    createMenuItem(App.localize("LeaveApplicationToDo"), "Pages.LeaveApplicationToDo", "text_snippet", "leaveapplicationtodo")
                ]),

                createMenuItem(App.localize("InvoiceRequest"), "Pages.InvoiceRequest", "request_quote", "invoicerequestlists", [
                    createMenuItem(App.localize("InvoiceRequestProject"), "Pages.Project.InvoiceRequest", "info", "projectinvoicelist"),
                    createMenuItem(App.localize("InvoiceRequestService"), "Pages.Service.InvoiceRequest", "info", "serviceinvoicelist")
                ]),
                createMenuItem("User Story Report", "Pages.Employeewise.UserStory", "contact_page", "empusersStoryReport"),

                createMenuItem(App.localize("Reports"), "Pages.Reports", "assessment", "", [
                    createMenuItem(App.localize("UserTimeSheet"), "Pages.Reports.UserTimesheet", "info", "reports"),
                    createMenuItem(App.localize("EmployeeLoginLogoutReport"), "Pages.Reports.EmployeeInOutReport", "info", "loginLogoutReport"),
                    createMenuItem(App.localize("ProjectReports"), "Pages.ProjectReport", "info", "projectreport"),
                    createMenuItem("Sales Project ", "Pages.Reports.Sales", "contact_page", "salesReport"),
                    createMenuItem("Sales Service", "Pages.Reports.Service", "contact_page", "salesService"),
                    createMenuItem("Collection Report", "Pages.Reports.CollectionReport", "contact_page", "collectionReport"),
                    createMenuItem("Invoice Report", "Pages.Reports.InvoiceReport", "contact_page", "invoiceReport"),
                    createMenuItem("Production Report", "Pages.Reports.ProductionReport", "contact_page", "productionReport"),
                    createMenuItem("Outstanding Invoice Report", "Pages.OutstandingInvoice.Report", "contact_page", "outstandingInvoice"),
                    createMenuItem("Outstanding Client Report", "Pages.OutstandingClient.Report", "contact_page", "outstandingClient"),
                    createMenuItem("GST Data Report", "Pages.GSTData.Report", "contact_page", "gstData"),
                    createMenuItem(App.localize("ProjectStats(Amount)"), "Pages.ProjectStatsAmount.Report", "info", "projectstatsamount"),
                    createMenuItem(App.localize("ProjectStatsHour"), "Pages.ProjectStatsHour.Report", "assessment", "projectstatehour"),
                    createMenuItem("User Story Report", "Pages.UserStoryReport", "contact_page", "usersStoryReport"),
                    createMenuItem("Opportunity Report", "Pages.Opportunity.Report", "contact_page", "opportunityReport"),
                    createMenuItem(App.localize("Reports_ProjectWiseTimesheet"), "Pages.Reports.ProjectWiseTimesheet", "info", "projectwisetimesheet"),
                    createMenuItem("Expense Report ", "Pages.Reports.Expense", "", "expenseReport"),
                    createMenuItem(App.localize("LeaveApplicationReport"), "Pages.Reports.LeaveApplicationReport", "", "applicationReport"),
                    createMenuItem(App.localize("DailySalesActivityReport"), "Pages.Reports.DailySalesActivityReport", "", "dailySalesActivityReport"),
                    createMenuItem("Closing Report", "Pages.Opportunity.Closing_Report", "contact_page", "opportunityClosingReport"),
                    createMenuItem("Daily Activity Report", "Pages.Reports.DailyActivityReport", "", "dailyActivityReport"),
                ]),
                createMenuItem("Knowledge Center", "Pages.KnowledgeCenter", "psychology", "manageknowledgecenter"),
                createMenuItem(App.localize("Document"), "Pages.Employee.Documents", "description", "documentlist"),
                createMenuItem("Holiday", "Pages.Employee.Holiday", "accessibility", "holidaylist"),
                createMenuItem("Notification", "Pages.Notification", "info", "notification"),
                createMenuItem(App.localize("Settings"), "Pages.Settings", "info", "settings"),
                createMenuItem("Server Jobs", "Pages.Admin.HangfireDashboard", "info", "http://planning.meghtechnologies.in/hangfire"),
                createMenuItem("Audit Log", "Pages.AdministrationAuditLogs", "inventory", "auditlogs"),
                createMenuItem(App.localize("Utility"), "Pages.Utility", "info", "utility"),
                createMenuItem(App.localize("Change Password"), "", "password", "passwordChange"),
            ];

            vm.showMenuItem = function (menuItem) {
                if (menuItem.permissionName) {
                    return abp.auth.isGranted(menuItem.permissionName);
                }

                return true;
            }

            function createMenuItem(name, permissionName, icon, route, childItems) {
                return {
                    name: name,
                    permissionName: permissionName,
                    icon: icon,
                    route: route,
                    items: childItems
                };
            }
        }
    ]);
})();