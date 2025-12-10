using Microsoft.Playwright;
using WiseMigrateTests.TestHooks;
using Xunit;

namespace WiseMigrateTests.TestHooks
{
    public class TestBaseFixture : IClassFixture<GlobalTestFixture>, IAsyncLifetime
    {
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            Context = await GlobalTestFixture.Browser!.NewContextAsync();
            Page = await Context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await Context.CloseAsync();
        }
    }
}