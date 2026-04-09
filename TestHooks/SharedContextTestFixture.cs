using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;

namespace WiseMigrateTests.TestHooks
{
    public class SharedContextFixture : IAsyncLifetime
    {
        public IBrowserContext? Context { get; private set; }
        public IPage? Page { get; private set; }
        public string? ProjectName { get; private set; }
        private bool _isInitialized = false;

        public async Task EnsureInitializedAsync(IBrowser browser, Func<IPage, Task<string>> setupLogic)
        {
            if (_isInitialized) return;

            Context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1024, Height = 768 }
            });
            Page = await Context.NewPageAsync();
            
            ProjectName = await setupLogic(Page!); 
            _isInitialized = true;
        }

        public async Task InitializeAsync() => await Task.CompletedTask;

        public async Task DeleteSharedProjectAsync(IPage Page)
        {
            await Page.WaitForSelectorAsync("tr", new() { Timeout = 30000 });

            
            var rows = Page.Locator("tr");
            var rowCount = await rows.CountAsync();

            if (rowCount == 0)
            {
                Console.WriteLine("No projects found to delete.");
                return;
            }

            Console.WriteLine($"Found {rowCount} projects. Starting deletion...");
            var _basicSetup = new BasicSetup(Page);

            for (int i = rowCount - 1; i >= 0; i--)
            {
                try
                {
                    rows = Page.Locator("tr");
                    var currentRow = rows.Nth(i);

                    var projectName = await Page.GetByText("TC_").First.TextContentAsync();
                    if (string.IsNullOrWhiteSpace(projectName))
                    {
                        Console.WriteLine($"Warning: Could not extract project name from row {i}. Skipping.");
                        continue;
                    }

                    projectName = projectName.Trim();

                    Console.WriteLine($"Deleting project: {projectName}");
                    await _basicSetup.DeleteProjectAsync(projectName);
                    await _basicSetup.WaitForPageAsync(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting project at index {i}: {ex.Message}");
                }
            }

            Console.WriteLine("All projects deletion process completed.");
        }
        public async Task DisposeAsync()
        {
           Console.WriteLine("SharedContextFixture.DisposeAsync() - Cleaning up shared project");

            if (!string.IsNullOrEmpty(ProjectName) && Context != null)
            {
                var cleanupPage = await Context.NewPageAsync();
                try
                {
                    
                    await cleanupPage.GotoAsync(Utils.ConfigReader.Get("AppUrl") + "/projects", new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle,
                        Timeout = 60000
                    });

                    await DeleteSharedProjectAsync(cleanupPage);

                    Console.WriteLine($"[SHARED CLEANUP] Successfully deleted projects");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SHARED CLEANUP] Failed to delete projects : {ex.Message}");
                }
                finally
                {
                    if (cleanupPage != null)
                    {
                        await cleanupPage.CloseAsync();
                    }
                }
            }
            if (Context != null) await Context.CloseAsync();
        }
    }
}