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

        public async Task WaitForDashboardAsync()
        {
            await _page.WaitForURLAsync("**/projects");
        }

        public async Task ClickCreateNewProjectAsync()
        {
            var btn = _page.GetByRole(AriaRole.Button, new() { Name = "CREATE NEW PROJECT" });
            await btn.WaitForAsync();
            await btn.ClickAsync();
        }

        public async Task AssertProjectVisible(string projectName)
        {
            await _page.Locator("tr", new() { HasText = projectName }).WaitForAsync();
        }
    }
}
