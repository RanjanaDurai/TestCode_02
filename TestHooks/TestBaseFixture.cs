using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.Pages.Login;      
using WiseMigrateTests.Pages.Project;
using System.Collections.Generic;
using System.Reflection;
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
        protected virtual bool UseClassLevelGrouping => false;
        private static readonly Dictionary<Type, SharedContextFixture> _classFixtures = new();

        private static readonly Dictionary<Type, int> _testsExecuted = new Dictionary<Type, int>();
        private static readonly Dictionary<Type, int> _totalTestsInClass = new Dictionary<Type, int>();
        public async Task InitializeAsync()
        {
            var currentClassType = this.GetType();

            if (!_totalTestsInClass.ContainsKey(currentClassType))
            {
                _totalTestsInClass[currentClassType] = GetTestMethodCount(currentClassType);
                Console.WriteLine($"[REFLECTION] Found {_totalTestsInClass[currentClassType]} test methods in class {currentClassType.Name}");
            }

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

         private static int GetTestMethodCount(Type testClassType)
        {
            return testClassType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                .Count(method =>
                                    method.GetCustomAttributes(typeof(FactAttribute), true).Any() ||
                                    method.GetCustomAttributes(typeof(TheoryAttribute), true).Any());
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
                    var classType = this.GetType();                                 // Line A

                    _testsExecuted[classType] = _testsExecuted.GetValueOrDefault(classType) + 1;   // Line B

                    int completed = _testsExecuted[classType];                     // Line C
                    int total = _totalTestsInClass.GetValueOrDefault(classType, 1); // Line D

                    Console.WriteLine($"DISPOSEASYNC - Class Level Reset | Tests: {completed}/{total}");
                    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions
                    {
                        Timeout = 1000
                    });
                    

                    if (completed == total)                                         // Line E
                    {
                        Console.WriteLine(">>> FINAL CLASS CLEANUP - Deleting shared project after last test <<<");

                        if (_classFixtures.TryGetValue(classType, out var shared) && shared?.ProjectName != null)
                        {
                            try
                            {
                                var basicSetup = new BasicSetup(shared.Page!);
                                Page.GotoAsync(basicSetup.AppUrl + "/projects", new()
                                {
                                    WaitUntil = WaitUntilState.NetworkIdle,
                                    Timeout = 60000
                                }).GetAwaiter().GetResult();
                                await basicSetup.DeleteProjectAsync(shared.ProjectName);

                                Console.WriteLine($"[SHARED CLEANUP] Successfully deleted project: {shared.ProjectName}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[SHARED CLEANUP] Failed to delete '{shared.ProjectName}': {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Cleanup skipped - waiting for remaining tests ({completed}/{total})");
                    }

                    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 5000 });
        
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



