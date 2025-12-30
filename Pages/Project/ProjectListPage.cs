using Microsoft.Playwright;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectListPage
    {
        private readonly IPage _page;

        public ProjectListPage(IPage page)
        {
            _page = page;
        }

        public ILocator CreateNewProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "CREATE NEW PROJECT" });

        public async Task WaitForDashboardAsync()
        {
            await _page.WaitForURLAsync("**/projects");
        }

        public async Task ClickCreateNewProjectAsync()
        {
            await CreateNewProjectButton.WaitForAsync();
            await CreateNewProjectButton.ClickAsync();
        }

        public async Task AssertProjectVisibleAsync(string projectName)
        {
            await _page.Locator("tr", new() { HasText = projectName }).WaitForAsync();
        }
    }
}
