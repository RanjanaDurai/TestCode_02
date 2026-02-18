using Microsoft.Playwright;
using WiseMigrateTests.Pages.PreRequisites;
using WiseMigrateTests.TestHooks;
using Xunit;

namespace WiseMigrateTests.TestHooks
{
    
    public class TestBaseFixture : IClassFixture<GlobalTestFixture>, IAsyncLifetime
    {
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;
        protected BasicSetup _basicSetup= null!;

         

        public async Task InitializeAsync()
        {
            Context = await GlobalTestFixture.Browser!.NewContextAsync();
            Page = await Context.NewPageAsync();
            _basicSetup = new BasicSetup(Page);
        }

        public async Task DisposeAsync()
        {   
                await Page.GotoAsync(_basicSetup.AppUrl + "/projects");
                var projectName=BasicSetup.projectName;
                await _basicSetup.DeleteProjectAsync(projectName);
                await Context.CloseAsync();
        }
    }
}