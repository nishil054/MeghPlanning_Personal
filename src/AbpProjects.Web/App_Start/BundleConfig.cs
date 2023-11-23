using System.Web.Optimization;

namespace AbpProjects.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //ACCOUNT BUNDLES
            bundles.Add(
                new StyleBundle("~/Bundles/account-vendor/css")
                    .Include("~/fonts/roboto/roboto.css", new CssRewriteUrlTransform())
                    .Include("~/fonts/material-icons/materialicons.css", new CssRewriteUrlTransform())
                    .Include("~/lib/bootstrap/dist/css/bootstrap.css", new CssRewriteUrlTransform())
                    //===========Angular UI Grid
                    .Include("~/lib/angular-ui-grid/ui-grid.min.css", new CssRewriteUrlTransform())
                    //===========Angular UI Grid
                    //===========Angular Date Range Picker
                    .Include("~/lib/bootstrap-daterangepicker/daterangepicker.css", new CssRewriteUrlTransform())
                    //===========Angular Date Range Picker
                    .Include("~/lib/toastr/toastr.css", new CssRewriteUrlTransform())
                    .Include("~/lib/famfamfam-flags/dist/sprite/famfamfam-flags.css", new CssRewriteUrlTransform())
                    .Include("~/lib/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
                    .Include("~/lib/Waves/dist/waves.css", new CssRewriteUrlTransform())
                    .Include("~/lib/animate.css/animate.css", new CssRewriteUrlTransform())
                    .Include("~/css/materialize.css", new CssRewriteUrlTransform())
                    .Include("~/css/style.css", new CssRewriteUrlTransform())
                    .Include("~/Views/Account/_Layout.css", new CssRewriteUrlTransform())
            );

            bundles.Add(
                new ScriptBundle("~/Bundles/account-vendor/js/bottom")
                    .Include(
                        "~/lib/json2/json2.js",
                        "~/lib/jquery/dist/jquery.js",
                        "~/lib/bootstrap/dist/js/bootstrap.js",
                        "~/lib/moment/min/moment-with-locales.js",
                        "~/lib/jquery-validation/dist/jquery.validate.js",
                        "~/lib/blockUI/jquery.blockUI.js",
                        "~/lib/toastr/toastr.js",
                        "~/lib/sweetalert/dist/sweetalert.min.js",
                        "~/lib/spin.js/spin.js",
                        "~/lib/spin.js/jquery.spin.js",
                        "~/lib/Waves/dist/waves.js",
                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                        "~/js/admin.js",
                        "~/js/main.js"
                    )
            );

            //VENDOR RESOURCES

            //~/Bundles/App/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/App/vendor/css")
                    .Include("~/fonts/roboto/roboto.css", new CssRewriteUrlTransform())
                    .Include("~/fonts/material-icons/materialicons.css", new CssRewriteUrlTransform())
                    .Include("~/lib/bootstrap/dist/css/bootstrap.css", new CssRewriteUrlTransform())
                    //===========Angular UI Grid
                    .Include("~/lib/angular-ui-grid/ui-grid.min.css", new CssRewriteUrlTransform())
                    //===========Angular UI Grid
                    //===========Angular Date Range Picker
                    .Include("~/lib/bootstrap-daterangepicker/daterangepicker.css", new CssRewriteUrlTransform())
                    //===========Angular Date Range Picker
                    .Include("~/lib/toastr/toastr.css", new CssRewriteUrlTransform())
                    .Include("~/lib/famfamfam-flags/dist/sprite/famfamfam-flags.css", new CssRewriteUrlTransform())
                    .Include("~/lib/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
                    .Include("~/lib/Waves/dist/waves.css", new CssRewriteUrlTransform())
                    .Include("~/lib/animate.css/animate.css", new CssRewriteUrlTransform())
                    .Include("~/css/materialize.css", new CssRewriteUrlTransform())
                    .Include("~/css/style.css", new CssRewriteUrlTransform())
                    .Include("~/css/themes/all-themes.css", new CssRewriteUrlTransform())
                );

            //~/Bundles/App/vendor/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/vendor/js")
                    .Include(
                        "~/lib/jquery/dist/jquery.js",
                        "~/lib/json2/json2.js",
                        "~/lib/bootstrap/dist/js/bootstrap.js",
                        //===========Angular Moment Date
                        "~/lib/moment/min/moment-with-locales.js",
                        "~/lib/moment/min/moment-timezone-with-data.js",
                        //===========Angular Moment Date
                        "~/lib/jquery-validation/dist/jquery.validate.js",
                        "~/lib/blockUI/jquery.blockUI.js",
                        "~/lib/toastr/toastr.js",
                        "~/lib/sweetalert/dist/sweetalert.min.js",
                        "~/lib/spin.js/spin.js",
                        "~/lib/spin.js/jquery.spin.js",
                        //===========Angular Date Range Picker
                        "~/lib/bootstrap-daterangepicker/daterangepicker.js",
                        //===========Angular Date Range Picker
                        "~/lib/bootstrap-select/dist/js/bootstrap-select.js",
                        "~/lib/jquery-slimscroll/jquery.slimscroll.js",
                        "~/lib/Waves/dist/waves.js",
                        "~/lib/push.js/push.js",
                        "~/lib/jquery.serializejson/jquery.serializejson.js",
                        "~/Scripts/angular.min.js",
                        "~/Scripts/angular-animate.min.js",
                        "~/Scripts/angular-sanitize.min.js",
                        //===========Angular UI Grid
                        "~/lib/angular-ui-grid/ui-grid.min.js",
                        //===========Angular UI Grid
                        //===========Angular Date Range Picker
                        "~/Scripts/angular-daterangepicker/angular-daterangepicker.min.js",
                        //===========Angular Date Range Picker
                        //===========Angular Moment Date
                        "~/Scripts/angular-moment/angular-moment.min.js",
                        //===========Angular Moment Date
                        "~/Scripts/angular-ui-router.min.js",
                        "~/Scripts/angular-ui/ui-bootstrap.min.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                        "~/Scripts/angular-ui/ui-utils.min.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                        "~/Abp/Framework/scripts/libs/angularjs/abp.ng.js",
                        "~/Abp/Framework/scripts/libs/abp.moment.js",
                        "~/js/admin.js",
                        "~/Scripts/jquery.signalR-2.4.1.min.js",
                        "~/js/main.js",
                        "~/js/common.js",
                        "~/js/angucomplete-alt.js"
                    )
                );

            //Home-Index Bundles
            bundles.Add(
                new ScriptBundle("~/Bundles/home-index")
                    .Include(
                        "~/lib/jquery-countTo/jquery.countTo.js",
                        "~/lib/raphael/raphael.js",
                        "~/lib/morris.js/morris.js",
                        "~/lib/chart.js/dist/Chart.bundle.js",
                        "~/lib/Flot/jquery.flot.js",
                        "~/lib/Flot/jquery.flot.resize.js",
                        "~/lib/Flot/jquery.flot.pie.js",
                        "~/lib/Flot/jquery.flot.categories.js",
                        "~/lib/Flot/jquery.flot.time.js",
                        "~/lib/jquery-sparkline/dist/jquery.sparkline.js"
                    )
            );

            //APPLICATION RESOURCES

            //~/Bundles/App/Main/css
            bundles.Add(
                new StyleBundle("~/Bundles/App/Main/css")
                    .IncludeDirectory("~/App/Main", "*.css", true)
                );

            //~/Bundles/App/Main/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/Main/js")
                    .IncludeDirectory("~/Common/Scripts", "*.js", true)
                    .IncludeDirectory("~/App/Main", "*.js", true)
                );



            #region Permission related 

            //METRONIC
            bundles.Add(
               new StyleBundle("~/Bundles/App/metronic/css")
                   .Include("~/metronic/assets/global/css/plugins-md.rtl.min.css")
                   .Include("~/metronic/assets/global/css/plugins-md.rtl.css")
                    .Include("~/metronic/assets/global/css/plugins-md.css")
                     .Include("~/metronic/assets/global/css/plugins-md.css")
                      .Include("~/metronic/assets/global/css/plugins-md.min.css")
                       .Include("~/metronic/assets/global/css/plugins-rtl.css")
                        .Include("~/metronic/assets/global/css/plugins.css")

               );


            bundles.Add(
              new ScriptBundle("~/Bundles/App/metronic/js")
                  .Include(
                      "~/metronic/assets/global/scripts/app.js",
                      "~/metronic/assets/admin/layout4/scripts/layout.js",
                      "~/metronic/assets/layouts/global/scripts/quick-sidebar.js"
                  )
              );
            bundles.Add(
                new StyleBundle("~/Bundles/App/css/themes")

                    .Include("~/css/themes/style.css")

                );
            #endregion
        }
    }
}