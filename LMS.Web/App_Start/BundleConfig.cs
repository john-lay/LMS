// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="">
//   
// </copyright>
// <summary>
//   The bundle config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web
{
    using System.Web.Optimization;

    using BundleTransformer.Core.Bundles;
    using BundleTransformer.Core.Orderers;

    /// <summary>
    /// The bundle config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// The bootstrap path.
        /// </summary>
        public const string BootstrapPath = "~/Bundles/Bootstrap";

        /// <summary>
        /// The register bundles.
        /// </summary>
        /// <param name="bundles">
        /// The bundles.
        /// </param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // bundles.Add(new ScriptBundle("~/js").Include(
            // "~/Scripts/jquery-{version}.js",
            // "~/Scripts/bootstrap.js",
            // "~/Scripts/jquery.validate.js",
            // "~/scripts/jquery.validate.unobtrusive.js",
            // "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"));
            bundles.Add(new ScriptBundle("~/js").Include("~/Scripts/Library/jquery-1.11.1.min.js", "~/Scripts/Library/angular/angular.min.js", "~/Scripts/Library/bootstrap/bootstrap.js"));

            var commonStylesBundle = new CustomStyleBundle(BootstrapPath);
            commonStylesBundle.Orderer = new NullOrderer();

            commonStylesBundle.Include("~/Content/bootstrap/bootstrap.less", "~/Content/datepicker.css");

            bundles.Add(commonStylesBundle);
        }
    }
}