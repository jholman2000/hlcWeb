﻿using System.Web.Optimization;

namespace hlcWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/underscore-min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-toggle.min.js",
                        "~/Scripts/respond.min.js",
                        "~/Scripts/bootbox.min.js",
                        "~/Scripts/jquery.tablesorter.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-toggle.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/animate.min.css",
                      "~/Content/site.css"));
        }
    }
}
