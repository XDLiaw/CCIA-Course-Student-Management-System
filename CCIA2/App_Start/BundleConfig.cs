using System.Web;
using System.Web.Optimization;

namespace CCIA2
{
    public class BundleConfig
    {
        // 如需 Bundling 的詳細資訊，請造訪 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            // bootstrap & 自訂樣式
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap-3.3.4.css",
                        "~/Content/default.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap-3.3.4.js"));

            // 日曆盒
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                        "~/Scripts/datetimepicker/jquery.datetimepicker.full.js",
                        "~/Scripts/datetimepicker/calendarBox.js"));
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                        "~/Content/jquery.datetimepicker.css"));

            // 多選元件
            bundles.Add(new ScriptBundle("~/bundles/Chosen").Include(
                        "~/Scripts/Chosen/chosen.jquery.min.js"));
            bundles.Add(new StyleBundle("~/Content/Chosen").Include(
                        "~/Content/Chosen/chosen.css"));

            // 檔案瀏覽按鈕
            bundles.Add(new StyleBundle("~/Content/fileBrowseBtn").Include(
                   "~/Content/btn.file.css"));  
        }
    }
}