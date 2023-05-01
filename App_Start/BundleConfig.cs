using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Transactiondetails.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));


            //bundles.Add(new ScriptBundle("~/Scripts").Include("~/Scripts/dist"));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));
            //bundles.Add(new StyleBundle("~/Content"));
            //bundles.Add(new StyleBundle("~/Css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            // jquery datataables js files
            bundles.Add(new ScriptBundle("~/bundles/dt/datatables").Include(
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js"));

            // jquery datatables css file
            bundles.Add(new StyleBundle("~/Content/dt/datatables").Include(
                      "~/Content/DataTables/css/dataTables.bootstrap.css"));

            //bundles.Add(new StyleBundle("~/assets/vendor/js").Include(
            //         "~/assets/vendor/apexcharts/apexcharts.min.js",
            //         "~/assets/vendor/bootstrap/js/bootstrap.bundle.min.js",
            //         "~/assets/vendor/chart.js/chart.umd.js",
            //         "~/assets/vendor/echarts/echarts.min.js",
            //         "~/assets/vendor/quill/quill.min.js",
            //         "~/assets/vendor/simple-datatables/simple-datatables.js",
            //         "~/assets/vendor/tinymce/tinymce.min.js",
            //         "~/assets/vendor/php-email-form/validate.js"));

            //bundles.Add(new StyleBundle("~/assets/vendor/css").Include(
            //          "~/assets/vendor/bootstrap/css/bootstrap.min.css",
            //          "~/assets/vendor/bootstrap-icons/bootstrap-icons.css",
            //          "~/assets/vendor/boxicons/css/boxicons.min.css",
            //          "~/assets/vendor/quill/quill.snow.css",
            //          "~/assets/vendor/quill/quill.bubble.css",
            //          "~/assets/vendor/remixicon/remixicon.css",
            //          "~/assets/vendor/simple-datatables/style.css",
            //          "~/assets/css/sweetalert2.min.css"));

            BundleTable.EnableOptimizations = true;

        }
    }
}
