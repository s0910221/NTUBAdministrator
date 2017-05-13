using System.Web;
using System.Web.Optimization;

namespace NTUBAdministrator
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //jQueryUI
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            //左邊選項script
            bundles.Add(new ScriptBundle("~/TreeMenu/js").Include(
                        "~/Scripts/exMenu.js"));

            //DateTimePicker
            bundles.Add(new ScriptBundle("~/DateTimePicker/js").Include(
                        "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/DateTimePicker/css").Include(
                        "~/Content/bootstrap-datetimepicker.css"));

            //行事曆
            bundles.Add(new ScriptBundle("~/Calendar/js").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/fullcalendar/fullcalendar.js",
                        "~/Scripts/fullcalendar/locale-all.js"));

            bundles.Add(new StyleBundle("~/Calendar/css").Include(
                        "~/Content/fullcalendar.css"));

        }
    }
}
