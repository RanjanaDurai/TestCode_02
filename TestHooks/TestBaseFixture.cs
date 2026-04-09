using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;      
using WiseMigrateTests.Pages.Project;
using System.Collections.Generic;
using WiseMigrateTests.Utils;
using Xunit;

namespace WiseMigrateTests.TestHooks
{
    public class TestBaseFixture : IClassFixture<GlobalTestFixture>,IClassFixture<SharedContextFixture>, IAsyncLifetime
    {
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;
        protected BasicSetup _basicSetup = null!;
        protected AttachmentHelper _attachmentHelper = null!;
        protected string? _createdProjectName; 
        protected virtual bool UseClassLevelGrouping => false;
        private static readonly Dictionary<Type, SharedContextFixture> _classFixtures = new();
        public async Task InitializeAsync()
        {
            if (UseClassLevelGrouping)
            {
                var type = this.GetType();
                if (!_classFixtures.ContainsKey(type)) _classFixtures[type] = new SharedContextFixture();
                
                var shared = _classFixtures[type];
                await shared.EnsureInitializedAsync(GlobalTestFixture.Browser!, async (page) => 
                {
                    var setup = new BasicSetup(page);
                    var loginPage= new LoginPage(page);
                    var projectPage= new ProjectListPage(page);
                    await loginPage.ValidateSuperAdminAccount(page);
                    await projectPage.ClickCreateNewProjectAsync();
                    await setup.CreateProjectAsync();
                    await setup.InfrastructureUploadPageSetupAsync();
                    await setup.WaitForPageAsync(5);
           
                    return setup.projectName ?? "SharedProject";
                });

                Context = shared.Context!;
                Page = shared.Page!;
                _createdProjectName = shared.ProjectName;
            }
            else
            {
                Context = await GlobalTestFixture.Browser!.NewContextAsync( new BrowserNewContextOptions
                {
                    ViewportSize = new ViewportSize { Width = 1024, Height = 768 } 
                });
                Page = await Context.NewPageAsync();
            }
            _basicSetup = new BasicSetup(Page);
            _attachmentHelper = new AttachmentHelper(Context);       
    
        }

        protected async Task NavigateToPageAsync(string pagePath)
        {
            if (string.IsNullOrEmpty(pagePath))
                throw new ArgumentException("Page path cannot be null or empty", nameof(pagePath));

            try
            {
                // Automatically append current project ID if available
                string projectId = await ExtractProjectIdFromCurrentUrlAsync(Page);

                string fullUrl = $"{_basicSetup.AppUrl.TrimEnd('/')}{pagePath}?project_id={projectId}";

                Console.WriteLine($"Navigating to: {fullUrl}");

                await Page.GotoAsync(fullUrl, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 45000
                });

                Console.WriteLine($"Successfully loaded page: {pagePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation failed to {pagePath}: {ex.Message}");
                await ScreenshotHelper.TakeScreenshotAsync(Page, $"Nav_Fail_{pagePath.Replace("/", "_")}", "FAILED");
                throw;
            }
        }

        private static async Task<string> ExtractProjectIdFromCurrentUrlAsync(IPage page)
        {
            await page.WaitForURLAsync("**/*?project_id=*", new() { Timeout = 15000 });
            var uri = new Uri(page.Url);
            return System.Web.HttpUtility.ParseQueryString(uri.Query).Get("project_id") 
                ?? throw new InvalidOperationException("Could not extract project_id from URL");
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
                if (UseClassLevelGrouping)
                {
                    Console.WriteLine("DISPOSEASYNC - Class Level Reset (Shared Context)");                     
                    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions
                    {
                        Timeout = 10000
                    });
        
                }
                else
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
                
            }
            catch (Exception ex)
            {
            }
            
        }
    }
    
}



