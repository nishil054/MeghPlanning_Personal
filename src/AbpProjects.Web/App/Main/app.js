(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'ui.select2',
        'ui.router',
        'ui.bootstrap',
        'ui.jq',
        'ui.grid',
        'ui.grid.expandable',
        'ui.grid.pagination',
        'ui.grid.autoResize',
        'daterangepicker',
        'angularMoment',
        'moment-picker',
        'abp',
        'angucomplete-alt'

    ]);

    //Configuration for Angular UI routing.
    app.config([
        '$stateProvider', '$urlRouterProvider', '$locationProvider', '$qProvider',
        function ($stateProvider, $urlRouterProvider, $locationProvider, $qProvider) {
            $locationProvider.hashPrefix('');
            $urlRouterProvider.otherwise('/dashboard');
            $qProvider.errorOnUnhandledRejections(false);

            if (abp.auth.hasPermission('Pages.Users')) {
                $stateProvider
                    .state('users', {
                        url: '/employee',
                        templateUrl: '/App/Main/views/users/index.cshtml',
                        menu: 'Employee' //Matches to name of 'Users' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');

            }
            if (abp.auth.hasPermission('Pages.Roles')) {
                $stateProvider
                    .state('roles', {
                        url: '/roles',
                        templateUrl: '/App/Main/views/roles/index.cshtml',
                        menu: 'Roles' //Matches to name of 'Tenants' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Expense.Category')) {
                $stateProvider
                    .state('expcategorylist', {
                        url: '/expcategorylist',
                        templateUrl: '/App/Main/views/expcategorylist/index.cshtml',
                        menu: 'ExpenseCategory' //Matches to name of 'Tenants' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Expense.SubCategory')) {
                $stateProvider
                    .state('expensesubcategorylist', {
                        url: '/expensesubcategorylist',
                        templateUrl: '/App/Main/views/expensesubcategorylist/index.cshtml',
                        menu: 'ExpenseSubCategory' //Matches to name of 'Tenants' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Settings')) {
                $stateProvider
                    .state('settings', {
                        url: '/settings',
                        templateUrl: '/App/Main/views/settings/index.cshtml',
                        menu: 'Settings'
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Expense.Entry')) {
                $stateProvider
                    .state('expenseentrylist', {
                        url: '/expenseentrylist',
                        templateUrl: '/App/Main/views/expenseentrylist/index.cshtml',
                        menu: 'ExpenseEntry' //Matches to name of 'Tenants' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Tenants')) {
                $stateProvider
                    .state('tenants', {
                        url: '/tenants',
                        templateUrl: '/App/Main/views/tenants/index.cshtml',
                        menu: 'Tenants'
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.AdministrationAuditLogs')) {
                $stateProvider
                    .state('auditlogs', {
                        url: '/auditlog',
                        templateUrl: '/App/Main/views/auditlogs/index.cshtml',
                        menu: 'AuditLog' //Matches to name of 'Tenants' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/tenants');
            }
            if (abp.auth.hasPermission('Pages.Support')) {
                $stateProvider
                    .state('supportpages', {
                        url: '/manageservice',
                        templateUrl: '/App/Main/views/supportpages/index.cshtml',
                        menu: 'Support' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('detail', {
                        url: '/detail/:id?',
                        templateUrl: '/App/Main/views/supportpages/detail.cshtml',
                        menu: 'Detail' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            
            if (abp.auth.hasPermission('Pages.Notification')) {
                $stateProvider
                    .state('notification', {
                        url: '/notification',
                        templateUrl: '/App/Main/views/notification/index.cshtml',
                        menu: 'Notification' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            $stateProvider.state('notifications', {
                url: '/notifications',
                templateUrl: '/App/Main/views/notifications/index.cshtml'
            });

            if (abp.auth.hasPermission('Pages.Utility')) {
                $stateProvider
                    .state('utility', {
                        url: '/utility',
                        templateUrl: '/App/Main/views/utility/index.cshtml',
                        menu: 'Utility' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }


            $stateProvider
                .state('home', {
                    url: '/dashboard',
                    templateUrl: '/App/Main/views/home/home.cshtml',
                    menu: 'Home' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                })
           
           
            $stateProvider
                .state('servicesearch', {
                    url: '/servicesearch',
                    templateUrl: '/App/Main/views/supportpages/servicesearch.cshtml',
                    menu: 'Home' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                })
            if (abp.auth.hasPermission('Pages.DataVault')) {
                if (abp.auth.hasPermission('Pages.DataVault.Company')) {
                    $stateProvider
                        .state('company', {
                            url: '/company',
                            templateUrl: '/App/Main/views/company/index.cshtml',
                            menu: 'Company' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.ProjectType')) {
                    $stateProvider
                        .state('projecttype', {
                            url: '/projecttype',
                            templateUrl: '/App/Main/views/projecttype/index.cshtml',
                            menu: 'ProjectType' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.WorkType')) {
                    $stateProvider
                        .state('worktype', {
                            url: '/worktype',
                            templateUrl: '/App/Main/views/worktype/index.cshtml',
                            menu: 'WorkType' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.TypeName')) {
                    $stateProvider
                        .state('typename', {
                            url: '/typename',
                            templateUrl: '/App/Main/views/typename/index.cshtml',
                            menu: 'TypeName'
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.Category')) {
                    $stateProvider
                        .state('category', {
                            url: '/category',
                            templateUrl: '/App/Main/views/category/index.cshtml',
                            menu: 'category'
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.Admin.Leaves')) {
                    $stateProvider
                        .state('manageleaves', {
                            url: '/manageleaves',
                            templateUrl: '/App/Main/views/manageleaves/index.cshtml',
                            menu: 'manageleaves'
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.Admin.Holiday')) {
                    $stateProvider
                        .state('holiday', {
                            url: '/holiday',
                            templateUrl: '/App/Main/views/holiday/index.cshtml',
                            menu: 'Holiday' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.Admin.Documents')) {
                    $stateProvider
                        .state('document', {
                            url: '/document',
                            templateUrl: '/App/Main/views/document/index.cshtml',
                            menu: 'Document' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        });
                }
                if (abp.auth.hasPermission('Pages.DataVault.Financialyear')) {
                    $stateProvider
                        .state('financialyear', {
                            url: '/financialyear',
                            templateUrl: '/App/Main/views/financialyear/index.cshtml',
                            menu: 'Financialyear'
                        });
                    $urlRouterProvider.otherwise('/dashboard');
                }
                if (abp.auth.hasPermission('Pages.ManageCategory')) {
                $stateProvider
                    .state('nthlevelcategory', {
                        url: '/nthlevelcategory',
                        templateUrl: '/App/Main/views/nthlevelcategory/index.cshtml',
                        menu: 'nthlevelcategory' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('subcategory', {
                        url: '/subcategory/:id?',
                        templateUrl: '/App/Main/views/nthlevelcategory/subcategories.cshtml',
                        menu: 'subcategory' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                }
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Employee.Documents')) {
                $stateProvider
                    .state('documentlist', {
                        url: '/documentlist',
                        templateUrl: '/App/Main/views/documentlist/index.cshtml',
                        menu: 'Document' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Employee.Holiday')) {
                $stateProvider
                    .state('holidaylist', {
                        url: '/holidaylist',
                        templateUrl: '/App/Main/views/holidaylist/index.cshtml',
                        menu: 'Holiday'
                        //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            //if (abp.auth.hasPermission('Pages.Admin.HangfireDashboard')) {
            //    $stateProvider
            //        .state('hangfire', {
            //            url: '/hangfire',
            //            templateUrl: '/hangfire',
            //            menu: 'Server Jobs'
            //            //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
            //        });
            //    $urlRouterProvider.otherwise('/dashboard');
            //}
            if (abp.auth.hasPermission('Pages.Import')) {
                $stateProvider
                    .state('importftp', {
                        url: '/importftp',
                        templateUrl: '/App/Main/views/importftp/index.cshtml',
                        menu: 'importftp'
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Import')) {
                $stateProvider
                    .state('createftp', {
                        url: '/createftp',
                        templateUrl: '/App/Main/views/importftp/createFTPDetails.cshtml',
                        menu: 'createftp'

                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.KnowledgeCenter')) {
                $stateProvider
                    .state('manageknowledgecenter', {
                        url: '/manageknowledgecenter',
                        templateUrl: '/App/Main/views/manageknowledgecenter/index.cshtml',
                        menu: 'manageknowledgecenter'
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.UserStory')) {
                $stateProvider
                    .state('importuserstory', {
                        url: '/importuserstory/:id?',
                        templateUrl: '/App/Main/views/importuserstory/index.cshtml',
                        menu: 'importuserstory' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.UserStoryReport')) {
                $stateProvider
                    .state('usersStoryReport', {
                        url: '/usersStoryReport',
                        templateUrl: '/App/Main/views/importuserstory/userStoryReport.cshtml',
                        menu: 'UsersStoryReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Employeewise.UserStory')) {
                $stateProvider
                    .state('empusersStoryReport', {
                        url: '/empusersStoryReport',
                        templateUrl: '/App/Main/views/importuserstory/empuserStoryReport.cshtml',
                        menu: 'UsersStoryReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/');
            }
            if (abp.auth.hasPermission('Pages.InvoiceRequest')) {
                $stateProvider
                    .state('projectinvoicelist', {
                        url: '/projectinvoicerequest',
                        templateUrl: '/App/Main/views/invoicerequestlists/projectinvoicelist.cshtml',
                        menu: 'Invoice Request' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('serviceinvoicelist', {
                        url: '/serviceinvoicerequest',
                        templateUrl: '/App/Main/views/invoicerequestlists/serviceinvoicelist.cshtml',
                        menu: 'Invoice RequestService' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Clients')) {
                $stateProvider
                    .state('clients', {
                        url: '/clients',
                        templateUrl: '/App/Main/views/client/index.cshtml',
                        menu: 'Clients' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }

            $urlRouterProvider.otherwise('/dashboard');

            if (abp.auth.hasPermission('Pages.Opportunity')) {
                $stateProvider
                    .state('inquiry', {
                        url: '/inquiry',
                        templateUrl: '/App/Main/views/opportunity/inquiry.cshtml',
                        menu: 'Inquiry' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                   
                    .state('addFollow', {
                        url: '/addFollow/:id?/:name?/:catid?',
                        templateUrl: '/App/Main/views/opportunity/addFollow.cshtml',
                        menu: 'addFollow' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.GeneralOpportunity')) {
                $stateProvider
                    .state('opportunity', {
                        url: '/opportunity',
                        templateUrl: '/App/Main/views/opportunity/opportunity.cshtml',
                        menu: 'Opportunity' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.MyOpportunity')) {
                $stateProvider
                    .state('myopportunity', {
                        url: '/myopportunity',
                        templateUrl: '/App/Main/views/opportunity/myopportunity.cshtml',
                        menu: 'myopportunity' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Opportunity.Report')) {
                $stateProvider
                    .state('opportunityReport', {
                        url: '/opportunityReport',
                        templateUrl: '/App/Main/views/reports/opportunity.cshtml',
                        menu: 'opportunityReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('opportunityDetails', {
                        url: '/opportunityDetails/:id',
                        templateUrl: '/App/Main/views/reports/opportunityDetails.cshtml',
                        menu: 'Opportunity Details' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                });
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Opportunity.Closing_Report')) {
                $stateProvider
                    .state('opportunityClosingReport', {
                        url: '/opportunityClosingReport',
                        templateUrl: '/App/Main/views/reports/opportunityClosingReport.cshtml',
                        menu: 'opportunityClosingReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.LeaveApplicationReport')) {
                $stateProvider
                    .state('applicationReport', {
                        url: '/applicationReport',
                        templateUrl: '/App/Main/views/reports/applicationReport.cshtml',
                        menu: 'applicationReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');   
            }

            if (abp.auth.hasPermission('Pages.Reports.DailySalesActivityReport')) {
                $stateProvider
                    .state('dailySalesActivityReport', {
                        url: '/dailySalesActivityReport',
                        templateUrl: '/App/Main/views/reports/dailySalesActivityReport.cshtml',
                        menu: 'dailySalesActivityReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Reports.DailyActivityReport')) {
                $stateProvider
                    .state('dailyActivityReport', {
                        url: '/dailyActivityReport',
                        templateUrl: '/App/Main/views/reports/dailyActivityReport.cshtml',
                        menu: 'dailyActivityReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Project')) {
                $stateProvider
                    //.state('project', {
                    //    url: '/project',
                    //    templateUrl: '/App/Main/views/project/index.cshtml',
                    //    menu: 'Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    //})
                    .state('detailproject', {
                        url: '/detailproject/:id?/:pid?',
                        templateUrl: '/App/Main/views/reports/detail.cshtml',
                        menu: 'Detail' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('projectView', {
                        url: '/projectView/:id',
                        templateUrl: '/App/Main/views/project/view.cshtml',
                        menu: 'projectView' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('projectTypeList', {
                        url: '/projectTypeList/:id',
                        templateUrl: '/App/Main/views/project/projectTypeList.cshtml',
                        menu: 'projectTypeList' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.ActiveProjects')) {
                $stateProvider
                    .state('activeprojects', {
                        url: '/activeprojects',
                        templateUrl: '/App/Main/views/project/activeprojects.cshtml',
                        menu: 'Active Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.AllProjects')) {
                $stateProvider
                    .state('allprojects', {
                        url: '/allprojects',
                        templateUrl: '/App/Main/views/project/allprojects.cshtml',
                        menu: 'All Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.AMCProjects')) {
                $stateProvider
                    .state('amcprojects', {
                        url: '/amcprojects',
                        templateUrl: '/App/Main/views/project/amcprojects.cshtml',
                        menu: 'AMC Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.OnGoingProjects')) {
                $stateProvider
                    .state('ongoingprojects', {
                        url: '/ongoingprojects',
                        templateUrl: '/App/Main/views/project/ongoingprojects.cshtml',
                        menu: 'On Going Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.InvoiceCollectionProjects')) {
                $stateProvider
                    .state('invoicecollectionprojects', {
                        url: '/invoicecollectionprojects',
                        templateUrl: '/App/Main/views/project/invoicecollectionprojects.cshtml',
                        menu: 'Invoice / Collection Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.CompletedProjects')) {
                $stateProvider
                    .state('completedprojects', {
                        url: '/completedprojects',
                        templateUrl: '/App/Main/views/project/completedprojects.cshtml',
                        menu: 'Completed Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.OnHoldProjects')) {
                $stateProvider
                    .state('onholdprojects', {
                        url: '/onholdprojects',
                        templateUrl: '/App/Main/views/project/onholdprojects.cshtml',
                        menu: 'On Hold Projects' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.ProjectsWithoutClient')) {
                $stateProvider
                    .state('projectswithoutclient', {
                        url: '/projectswithoutclient',
                        templateUrl: '/App/Main/views/project/projectswithoutclient.cshtml',
                        menu: 'Projects Without Client' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                $urlRouterProvider.otherwise('/dashboard');
            }

            $stateProvider
                .state('about', {
                    url: '/about',
                    templateUrl: '/App/Main/views/about/about.cshtml',
                    menu: 'About' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                })

            if (abp.auth.hasPermission('Pages.Employee.VPS')) {
                $stateProvider
                    .state('vpslist', {
                        url: '/vpslist',
                        templateUrl: '/App/Main/views/vpslist/index.cshtml',
                        menu: 'VPS' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        // })
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Support.VPS')) {
                $stateProvider
                    .state('vps', {
                        url: '/vps',
                        templateUrl: '/App/Main/views/vps/index.cshtml',
                        menu: 'VPS' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider

                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.GSTDashboard')) {
                $stateProvider
                    .state('gstdashboard', {
                        url: '/gstdashboard',
                        templateUrl: '/App/Main/views/gstdashboard/index.cshtml',
                        menu: 'GSTDashboard' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.UserStory')) {
                $stateProvider
                    .state('userStory', {
                        url: '/userStory',
                        templateUrl: '/App/Main/views/importuserstory/userStory.cshtml',
                        menu: 'userStory' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
            }
            if (abp.auth.hasPermission('Pages.Employeewise.UserStory')) {
                $stateProvider
                    .state('empuserStory', {
                        url: '/empuserStory',
                        templateUrl: '/App/Main/views/importuserstory/empuserStory.cshtml',
                        menu: 'userStory' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
            }
            if (abp.auth.hasPermission('Pages.LeaveApplication')) {
                $stateProvider
                    .state('leaveapplication', {
                        url: '/leaveapplication',
                        templateUrl: '/App/Main/views/leaveApplication/index.cshtml',
                        menu: 'Leave Application' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.LeaveApplicationToDo')) {
                $stateProvider
                    .state('leaveapplicationtodo', {
                        url: '/leaveapplicationtodo',
                        templateUrl: '/App/Main/views/leaveApplicationToDo/index.cshtml',
                        menu: 'Leave Application TO DO' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            $stateProvider
                .state('timesheet', {
                    url: '/timesheet',
                    templateUrl: '/App/Main/views/timesheet/index.cshtml',
                    menu: 'TimeSheet' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                })
                //.state('detail', {
                //    url: '/detail/:id?',
                //    templateUrl: '/App/Main/views/supportpages/detail.cshtml',
                //    menu: 'Detail' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                //})

                .state('passwordChange', {
                    url: '/passwordChange',
                    templateUrl: '/App/Main/views/changePassword/changePassword.cshtml',
                    menu: 'passwordChange' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                })


            if (abp.auth.hasPermission('Pages.Reports.ProjectWiseTimesheet')) {
                $stateProvider
                    .state('projectwisetimesheet', {
                        url: '/projectwisetimesheet',
                        templateUrl: '/App/Main/views/reports/projectwisetimesheet.cshtml',
                        menu: 'ProjectWise Timesheet' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.UserTimesheet')) {
                $stateProvider
                    .state('reports', {
                        url: '/reports',
                        templateUrl: '/App/Main/views/reports/reports.cshtml',
                        menu: 'UserTime Sheet' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.ProjectStatsAmount.Report')) {
                $stateProvider
                    .state('projectstatsamount', {
                        url: '/projectstatsamount',
                        templateUrl: '/App/Main/views/reports/projectstatsamount.cshtml',
                        menu: 'Project Stats(Amount)' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Reports.Sales')) {
                $stateProvider
                    .state('salesReport', {
                        url: '/salesReport',
                        templateUrl: '/App/Main/views/reports/sales_project.cshtml',
                        menu: 'salesReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.Expense')) {
                $stateProvider
                    .state('expenseReport', {
                        url: '/expenseReport',
                        templateUrl: '/App/Main/views/reports/ExpenseEntryReport.cshtml',
                        menu: 'expenseReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.Service')) {
                $stateProvider
                    .state('salesService', {
                        url: '/salesService',
                        templateUrl: '/App/Main/views/reports/sales_service.cshtml',
                        menu: 'salesService' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.CollectionReport')) {
                $stateProvider
                    .state('collectionReport', {
                        url: '/collectionReport',
                        templateUrl: '/App/Main/views/reports/collectionreport.cshtml',
                        menu: 'collectionReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.InvoiceReport')) {
                $stateProvider
                    .state('invoiceReport', {
                        url: '/invoiceReport',
                        templateUrl: '/App/Main/views/reports/invoiceReport.cshtml',
                        menu: 'invoiceReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.Reports.ProductionReport')) {
                $stateProvider
                    .state('productionReport', {
                        url: '/productionReport',
                        templateUrl: '/App/Main/views/reports/productionReport.cshtml',
                        menu: 'productionReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.ProjectStatsHour.Report')) {
                $stateProvider
                    .state('projectstatehour', {
                        url: '/projectstatshour',
                        templateUrl: '/App/Main/views/reports/projectstatehour.cshtml',
                        menu: 'Project States(Hour)' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }

            if (abp.auth.hasPermission('Pages.Reports.EmployeeInOutReport')) {
                $stateProvider
                    .state('loginLogoutReport', {
                        url: '/loginLogoutReport',
                        templateUrl: '/App/Main/views/reports/loginLogoutReport.cshtml',
                        menu: 'LoginLogoutReport' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })
                    .state('inoutDetails', {
                        url: '/inoutDetails/:id?/:fromDate?/:toDate?',
                        /*url: '/inoutDetails',*/
                        templateUrl: '/App/Main/views/reports/inOutDetailsReport.cshtml',
                        menu: 'loginLogoutDetailsReport', //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                        //params: { id: null, fromDate: null, toDate: null }
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.OutstandingInvoice.Report')) {
                $stateProvider
                    .state('outstandingInvoice', {
                        url: '/outstandingInvoice/:id',
                        templateUrl: '/App/Main/views/reports/outstandingInvoice.cshtml',
                        menu: 'outstandingInvoice' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.OutstandingClient.Report')) {
                $stateProvider
                    .state('outstandingClient', {
                        url: '/outstandingClient',
                        templateUrl: '/App/Main/views/reports/outstandingClient.cshtml',
                        menu: 'outstandingClient' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.GSTData.Report')) {
                $stateProvider
                    .state('gstData', {
                        url: '/gstData',
                        templateUrl: '/App/Main/views/reports/gstData.cshtml',
                        menu: 'gstData' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            if (abp.auth.hasPermission('Pages.ProjectReport')) {
                $stateProvider
                    .state('projectreport', {
                        url: '/projectreport/:pid?',
                        templateUrl: '/App/Main/views/reports/projectreport.cshtml',
                        menu: 'Project Report' //Matches to name of 'About' menu in AbpProjectsNavigationProvider
                    });
                $urlRouterProvider.otherwise('/dashboard');
            }
            /*if (abp.auth.hasPermission('')) {*/
                $stateProvider
                    .state('servicerenewal', {
                        url: '/servicerenewal',
                        templateUrl: '/App/Main/views/supportpages/servicerenewal.cshtml',
                        menu: 'Service Renewal' //Matches to name of 'Home' menu in AbpProjectsNavigationProvider
                    })

                $urlRouterProvider.otherwise('/dashboard');
            //}
        }
    ]);

})();