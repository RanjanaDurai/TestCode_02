using Microsoft.Playwright;
using Xunit;

namespace WiseMigrateTests.TestHooks
{
    public class GlobalTestFixture : IAsyncLifetime
    {
        public static IPlaywright? Playwright { get; private set; }
        public static IBrowser? Browser { get; private set; }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
        }

        public async Task DisposeAsync()
        {
            if (Browser != null) await Browser.CloseAsync();
            Playwright?.Dispose();
        }
    }
}