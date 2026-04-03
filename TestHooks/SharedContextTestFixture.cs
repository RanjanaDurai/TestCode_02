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

        public async Task DisposeAsync()
        {
            if (Context != null) await Context.CloseAsync();
        }
    }
}