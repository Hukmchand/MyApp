using System.Web;
using System.Web.Optimization;

namespace AntiTheftClub
{
   public class BundleConfig
   {
      // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {
          bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js"));

          //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
          //           "~/Scripts/jquery-1.11.1.min.js"));

          

         bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                     "~/Scripts/jquery.validate*"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/assets/js/modernizr.custom.32033.js"));

         bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/bootstrap.js"
                   ));

         //bundles.Add(new ScriptBundle("~/bundles/countryDropdown").Include(
         //           "~/Scripts/utility/intlTelInput.js"
         //           ));

         bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css"
                   ));

         //bundles.Add(new StyleBundle("~/Content/countryDropdown").Include("~/Content/utility/intlTelInput.css"));

         // Set EnableOptimizations to false for debugging. For more information,
         // visit http://go.microsoft.com/fwlink/?LinkId=301862
         BundleTable.EnableOptimizations = true;
      }
   }
}
