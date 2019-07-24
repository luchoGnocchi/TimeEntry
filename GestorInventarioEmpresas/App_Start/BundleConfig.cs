using System.Web;
using System.Web.Optimization;

namespace GestorInventarioEmpresas
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //// Datepicker script
            bundles.Add(new ScriptBundle("~/bundles/datepicker/js").Include(
                      "~/Vendor/bootstrap-datepicker-master/js/bootstrap-datepicker.min.js",
                      "~/Vendor/bootstrap-datepicker-master/locales/bootstrap-datepicker.es.min.js"
                      ));

            //// Datepicker style
            bundles.Add(new StyleBundle("~/bundles/datepicker/css").Include(
                      "~/Vendor/bootstrap-datepicker-master/css/bootstrap-datepicker3.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/bundles/basics/css").Include(
               // Font Awesome icons style
               "~/Vendor/fontawesome/css/font-awesome.min.css", 
               // Bootstrap style
               "~/Vendor/bootstrap/css/bootstrap.min.css",
               // Bootstrap dialog
               "~/Vendor/bootstrap-dialog/bootstrap-dialog.min.css"));
           bundles.Add(new ScriptBundle("~/bundles/alerts").Include(
                   
                    "~/ModulosJs/AlertManager.js",
                  
                      "~/Content/notify/notify.js",
                      "~/ModulosJs/BlockUIManager.js",
                      "~/Content/SweetAlert2/SweetAlert2.js"));
        
 
                     
            bundles.Add(new ScriptBundle("~/bundles/basics/js").Include(
              "~/Vendor/core/core.min.js",
              "~/Vendor/jquery/jquery-2.2.0.min.js",
              "~/Vendor/bootstrap/js/bootstrap.min.js",
              "~/Vendor/blockui/jquery.blockUI.js",
              "~/Vendor/bootstrap-dialog/bootstrap-dialog.min.js",
              "~/Vendor/async/async.min.js",
              "~/Scripts/Common/CommonFunctions.js",
              "~/Scripts/Common/UtilJs.js",
              "~/Vendor/moment/moment.min.js",
              "~/Vendor/select2-3.5.2/select2.min.js"));

            bundles.Add(new StyleBundle("~/bundles/page/css").Include(
    "~/Vendor/datatables/media/css/dataTables.bootstrap.min.css", "~/Vendor/sweetalert/sweetalert.css",
    "~/Vendor/datatables-plugins/fixed-columns/css/fixedColumns.dataTables.min.css",
    "~/Vendor/datatables-plugins/Scroller/scroller.bootstrap.min.css",
    "~/Vendor/datatables-plugins/Select/select.bootstrap.min.css",
    "~/Vendor/select2-3.5.2/select2.css",
    "~/Vendor/select2-bootstrap/select2-bootstrap.css"));
      

        //// Datatables style
        bundles.Add(new StyleBundle("~/bundles/datatables/css").Include(
                      "~/Vendor/datatables/media/css/dataTables.bootstrap.min.css" ,
                      "~/Vendor/datatables-plugins/fixed-columns/css/fixedColumns.dataTables.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/datatables/js").Include(
                      "~/Vendor/datatables/media/js/jquery.dataTables.min.js",
                      "~/Vendor/datatables/media/js/dataTables.bootstrap.min.js",
                    
                      "~/Vendor/datatables-plugins/fixed-columns/js/fixedColumns.dataTables.min.js"
                      ));
        }

}  }
