using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace LMS.Web
{
    public static class BundleConfig
    {
        public const string BootstrapPath = "~/Bundles/Bootstrap";

        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/js").Include(
            //    "~/Scripts/jquery-{version}.js",
            //    "~/Scripts/bootstrap.js",
            //    "~/Scripts/jquery.validate.js",
            //    "~/scripts/jquery.validate.unobtrusive.js",
            //    "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/Library/jquery-1.11.1.min.js",
                "~/Scripts/Library/angular/angular.min.js",
                "~/Scripts/Library/bootstrap/bootstrap.js"));

            var commonStylesBundle = new CustomStyleBundle(BootstrapPath);
            commonStylesBundle.Orderer = new NullOrderer();

            commonStylesBundle.Include(
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/datepicker.css");

            bundles.Add(commonStylesBundle);

        }
    }
}