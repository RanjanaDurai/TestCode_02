// Utils/ScreenshotHelper.cs
using Microsoft.Playwright;
using System;
using System.IO;
using WiseMigrateTests.Utils;
using Allure.Net.Commons;

namespace WiseMigrateTests.Utils
{
    public static class ScreenshotHelper
    {
        private static readonly string ScreenshotDir;

        static ScreenshotHelper()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var relativePath = ConfigReader.Get("ScreenshotPath");
            ScreenshotDir = Path.Combine(projectRoot, relativePath);

            try
            {
                Directory.CreateDirectory(ScreenshotDir);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<string> TakeScreenshotAsync(IPage page, string testName, string? customName = null)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var safeName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));

            var baseFileName = string.IsNullOrEmpty(customName)
                ? $"{safeName}_"
                : $"{safeName}_{customName}_";

            var searchPattern = $"{baseFileName}*.png";

            try
            {
                var existingFiles = Directory.GetFiles(ScreenshotDir, searchPattern);
                foreach (var oldFile in existingFiles)
                {
                    try
                    {
                        File.Delete(oldFile);
                    }
                    catch (IOException) { } // Ignore if file is locked
                }
            }
            catch (Exception ex)
            {
            }
            

            var fileName = string.IsNullOrEmpty(customName)
                ? $"{safeName}_{timestamp}.png"
                : $"{safeName}_{customName}_{timestamp}.png";

            var fullPath = Path.Combine(ScreenshotDir, fileName);

            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = fullPath,
                FullPage = true
            });

            Console.WriteLine($"[SCREENSHOT] SAVED: {fullPath}");
            var fullAbsolutePath = Path.GetFullPath(fullPath);
            Logger.Info($"Screenshot saved: {fullAbsolutePath}");
            
            if (File.Exists(fullAbsolutePath))
            {
                try 
                {
                    AllureApi.AddAttachment(
                        $"{testName}.png",
                        "image/png",
                        fullAbsolutePath
                    
                    );

                    Logger.Info($"[SCREENSHOT] ATTACHED to Allure: {fullAbsolutePath}");
                }
                catch (Exception attachEx)
                {
                    Logger.Error($"Failed to attach screenshot to Allure: {attachEx.Message}");
                    Console.WriteLine($"Failed to attach screenshot: {attachEx.Message}");
                }
            }
            else
            {
                Logger.Warn($"Screenshot file not found for attachment: {fullPath}");
            }
            return fullPath;
        }
    }
}