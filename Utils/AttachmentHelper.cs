using Microsoft.Playwright;
using WiseMigrateTests.TestHooks;
using Allure.Net.Commons;

namespace WiseMigrateTests.Utils
{   
    public  class AttachmentHelper : TestBaseFixture
    {        
        private readonly IBrowserContext _context;
        private string? _currentTracePath;

        public AttachmentHelper(IBrowserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task StartTracingForTestAsync(string testName)
        {
            _currentTracePath = Path.Combine("traces", $"trace-{testName}-{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            await _context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
                Name = testName
            });

            Logger.Info($"Tracing started for test: {testName} → {_currentTracePath}");
        }

        public async Task StopAndAttachTraceIfFailedAsync(bool testFailed, string testName)
        {
            if (_context == null || string.IsNullOrEmpty(_currentTracePath))
                return;

            try
            {
                // Stop tracing and save to disk
                await _context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = _currentTracePath
                });

                Logger.Info($"Tracing STOPPED. Expected file: {Path.GetFullPath(_currentTracePath)}");

                //if (!testFailed)
                //{
                    // Attach trace to Allure report
                    if (File.Exists(_currentTracePath))
                    {
                        AllureApi.AddAttachment(
                            $"trace-{testName}.zip" ,
                            "application/zip",  
                            _currentTracePath                    
                                            
                        );

                        Logger.Info($"Trace file attached to Allure report: {Path.GetFullPath(_currentTracePath)}");
                    }
                    else
                    {
                        Logger.Warn($"Trace file not found for attachment: {Path.GetFullPath(_currentTracePath)}");
                    }
                /*}
                else
                {
                    // Optional: clean up successful test traces
                    try
                    {
                        File.Delete(_currentTracePath);
                        Logger.Info($"Trace file deleted (test passed): {_currentTracePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"Failed to delete trace file: {ex.Message}");
                    }
                }
                */
            }
            catch (Exception ex)
            {
                Logger.Error($"Tracing stop/attach FAILED for '{testName}': {ex.Message}");
                // Fallback: screenshot
                try
                {
                    var path = await ScreenshotHelper.TakeScreenshotAsync(Page, $"{testName}_TracingFailed");
                    AttachScreenshotToAllure(path, testName);
                }
                catch { }
            }
        }

        public async Task RunWithTracingAsync(Func<Task> testAction, string testName)
        {
            bool failed = false;

            try
            {
                await StartTracingForTestAsync(testName);

                await testAction();

                // Screenshot for PASSED test
                try
                {
                    var page = _context.Pages.FirstOrDefault();
                    if (page != null)
                    {
                        await ScreenshotHelper.TakeScreenshotAsync(page, testName, "PASSED");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Screenshot (PASS) failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                failed = true;

                Logger.Error($"Test '{testName}' failed: {ex.Message}");

                // Screenshot for FAILED test
                try
                {
                    var page = _context.Pages.FirstOrDefault();
                    if (page != null)
                    {
                        await ScreenshotHelper.TakeScreenshotAsync(page, testName, "FAILED");
                    }
                }
                catch (Exception screenshotEx)
                {
                    Logger.Warn($"Screenshot (FAIL) failed: {screenshotEx.Message}");
                }

                throw; // Important
            }
            finally
            {
                await StopAndAttachTraceIfFailedAsync(failed, testName);
            }
        }
        // public async Task RunWithTracingAsync(Func<Task> testAction, string testName)
        // {
        //     bool failed = false;

        //     try
        //     {
        //         await StartTracingForTestAsync(testName);
        //         await testAction();
        //     }
        //     catch (Exception ex)
        //     {
        //         failed = true;

        //         Logger.Error($"Test '{testName}' failed: {ex.Message}");

        //         //  TAKE SCREENSHOT ONLY WHEN FAILED
        //         try
        //         {
        //             var page = _context.Pages.FirstOrDefault();
        //             if (page != null)
        //             {
        //                 await ScreenshotHelper.TakeScreenshotAsync(page, testName, "FAILED");
        //             }
        //         }
        //         catch (Exception screenshotEx)
        //         {
        //             Logger.Warn($"Screenshot capture failed: {screenshotEx.Message}");
        //         }

        //         throw; // VERY IMPORTANT
        //     }
        //     finally
        //     {
        //         await StopAndAttachTraceIfFailedAsync(failed, testName);
        //     }
        // }

        /// <summary>
        /// Alternative: Attach an existing screenshot file to Allure (for manual calls).
        /// </summary>
        /// 
        public void AttachScreenshotToAllure(string screenshotPath, string testName)
        {
            if (File.Exists(screenshotPath))
            {
                AllureApi.AddAttachment(
                    screenshotPath,
                    "image/png",
                    $"screenshot-{testName}.png"
                );

                Logger.Info($"Screenshot attached to Allure: {screenshotPath}");
            }
            else
            {
                Logger.Warn($"Screenshot file not found: {screenshotPath}");
            }
        }

    }
}