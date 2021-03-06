﻿using System.Web;
using System.Web.Optimization;

namespace Server
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.js"));

			bundles.Add(new ScriptBundle("~/bundles/jquery_validate").Include(
						"~/Scripts/jquery.validate.js",
						"~/Scripts/jquery.validate.unobtrusive.js"
						));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/backupper.css",
                      "~/Content/bootstrap.css"));
        }
    }
}
