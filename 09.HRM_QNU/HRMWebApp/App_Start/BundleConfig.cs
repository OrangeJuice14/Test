using System.Web;
using System.Web.Optimization;

namespace HRMWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
    


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Libs/jqwidgets/styles/jqx.base.css",
                      "~/Libs/jqwidgets/styles/jqx.energyblue.css",
                       "~/Libs/kendo/kendo.common.min.css",
                        "~/Libs/kendo/kendo.metro.min.css"

                      ));

            bundles.Add(new ScriptBundle("~/bundles/basejs").Include(
            "~/scripts/jquery-1.10.2.min.js",
            "~/scripts/moment.js",
            "~/scripts/locale.js"));


            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/scripts/angular.js",
                      "~/scripts/angular-route.js",
                      "~/scripts/ui-router.js",
                      "~/Scripts/ui-bootstrap-0.10.0.min.js",
                      "~/Scripts/ui-bootstrap-tpls-0.10.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                   "~/app/app.js",
                   "~/app/services/chamcong/*.js",
                   "~/app/services/kpi/*.js",
                   "~/app/controllers/chamcong/*.js",
                   "~/app/controllers/kpi/*.js",
                   "~/app/directives/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqxwidgets").Include(
                   "~/Libs/jqwidgets/jqx-all.js",
                   "~/Libs/jqwidgets/jqxangular.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                   "~/Libs/kendo/kendo.all.min.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/other").Include(
                      "~/scripts/linq.min.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
