/*using ExtentReports;
using ExtentReports.Reporter;
using ExtentReports.Reporter.Configuration;
using System;
using System.IO;

namespace WiseMigrateTests.Utils
{
    public static class ExtentReportManager
    {
        private static ExtentReports? _extent;
        private static ExtentTest? _test;

        private static readonly string ReportPath = Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, 
            "Reports", "ExtentReport.html");

        public static void Init()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ReportPath)!);

            var htmlReporter = new ExtentHtmlReporter(ReportPath);
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Wise Migration Test Report";
            htmlReporter.Config.ReportName = "Playwright xUnit Results";

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }

        public static ExtentTest CreateTest(string name)
        {
            _test = _extent!.CreateTest(name);
            return _test;
        }

        public static ExtentTest GetTest() => _test!;

        public static void Flush()
        {
            _extent!.Flush();
            System.Diagnostics.Process.Start("explorer.exe", ReportPath);
        }
    }
}
*/