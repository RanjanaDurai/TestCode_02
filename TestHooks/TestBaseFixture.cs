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
            Console.WriteLine("DISPOSEASYNC STARTED");

            try
            {
                
                string projectToDelete = _createdProjectName ?? _basicSetup?.projectName ?? string.Empty;

                if (string.IsNullOrEmpty(projectToDelete))
                {
                }
                else if (Context != null)
                {
                    IPage? cleanupPage = null;
                    try
                    {
                        cleanupPage = await Context.NewPageAsync();

                        await cleanupPage.GotoAsync(_basicSetup.AppUrl + "/projects", new()
                        {
                            WaitUntil = WaitUntilState.NetworkIdle,
                            Timeout = 60000
                        });

                        await cleanupPage.WaitForSelectorAsync("text=Project Dashboard", new()
                        {
                            Timeout = 45000
                        });

                        var cleanupSetup = new BasicSetup(cleanupPage);

                        bool deleted = false;
                        for (int attempt = 1; attempt <= 3 && !deleted; attempt++)
                        {
                            await cleanupSetup.DeleteProjectAsync(projectToDelete);
                            deleted = true;
                        }

                        if (!deleted)
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        if (cleanupPage != null)
                        {
                            await cleanupPage.CloseAsync();
                        }
                    }
                }

                if (Context != null)
                {
                    await Context.CloseAsync();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}


