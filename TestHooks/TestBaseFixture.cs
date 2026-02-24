using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Utils;
using Xunit;

namespace WiseMigrateTests.TestHooks
{
    public class TestBaseFixture : IClassFixture<GlobalTestFixture>, IAsyncLifetime
    {
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;
        protected BasicSetup _basicSetup = null!;
        protected AttachmentHelper _attachmentHelper = null!;
        protected string? _createdProjectName;

        public async Task InitializeAsync()
        {
            Context = await GlobalTestFixture.Browser!.NewContextAsync();
            Page = await Context.NewPageAsync();

            _basicSetup = new BasicSetup(Page);
            _attachmentHelper = new AttachmentHelper(Context);
        }

        protected async Task RunTestAsync(Func<Task> testBody, string testName)
        {
            try
            {
                await _attachmentHelper.RunWithTracingAsync(async () =>
                {
                    await testBody();
                }, testName);
            }
            catch (Exception ex)
            {
                await ScreenshotHelper.TakeScreenshotAsync(Page, testName, "FAILED");

                Logger.Error($"Test Failed: {testName}");
                Logger.Error(ex.ToString());

                throw; 
            }
        }

        
        public async Task DisposeAsync()
        {
            try
            {
                Logger.Info($"[DISPOSE] Starting cleanup for: {_createdProjectName}");

                if (Context != null)
                {
                    if (!string.IsNullOrEmpty(_createdProjectName))
                    {
                        var cleanupPage = await Context.NewPageAsync();

                        try
                        {
                            await cleanupPage.GotoAsync(
                                _basicSetup.AppUrl + "/projects",
                                new() { WaitUntil = WaitUntilState.NetworkIdle });

                            var cleanupSetup = new BasicSetup(cleanupPage);

                            await cleanupSetup.DeleteProjectAsync(_createdProjectName);
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn($"[DISPOSE] Cleanup failed: {ex.Message}");
                        }
                        finally
                        {
                            await cleanupPage.CloseAsync();
                        }
                    }

                    await Context.CloseAsync();
                }

                Logger.Info("[DISPOSE] Context closed successfully.");
            }
            catch (Exception ex)
            {
                Logger.Warn($"[DISPOSE] Dispose error: {ex.Message}");
            }
        }
    }
}



// main
// using Microsoft.Playwright;
// using WiseMigrateTests.Pages.PreRequisites;
// using WiseMigrateTests.TestHooks;
// using Xunit;

// namespace WiseMigrateTests.TestHooks
// {
    
//     public class TestBaseFixture : IClassFixture<GlobalTestFixture>, IAsyncLifetime
//     {
//         protected IBrowserContext Context { get; private set; } = null!;
//         protected IPage Page { get; private set; } = null!;
//         protected BasicSetup _basicSetup= null!;

         

//         public async Task InitializeAsync()
//         {
//             Context = await GlobalTestFixture.Browser!.NewContextAsync();
//             Page = await Context.NewPageAsync();
//             _basicSetup = new BasicSetup(Page);
//         }

//         public async Task DisposeAsync()
//         {   
//                 await Page.GotoAsync(_basicSetup.AppUrl + "/projects");
//                 var projectName=BasicSetup.projectName;
//                 await _basicSetup.DeleteProjectAsync(projectName);
//                 await Context.CloseAsync();
//         }

        
//     }
// }

// public async Task DisposeAsync()
        // {
        //     try
        //     {
        //         await Context.CloseAsync();
        //     }
        //     catch
        //     {
        //         // ignore cleanup errors
        //     }
        // }


    //     public async Task DisposeAsync()
    //     {
    //         try
    //         {
    //             if (_basicSetup != null && !string.IsNullOrEmpty(BasicSetup.projectName))
    //             {
    //                 await Page.GotoAsync(_basicSetup.AppUrl + "/projects");
    //                 await _basicSetup.DeleteProjectAsync(BasicSetup.projectName);
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             Logger.Warn($"Cleanup failed: {ex.Message}");
    //         }
    //         finally
    //         {
    //             try
    //             {
    //                 await Context.CloseAsync();
    //             }
    //             catch { }
    //         }
    //     }
    // }
// }
