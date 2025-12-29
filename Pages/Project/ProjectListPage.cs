using Microsoft.Playwright;
using System.Threading.Tasks;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectListPage
    {
        private readonly IPage _page;

        public ProjectListPage(IPage page)
        {
            _page = page;
        }

        private ILocator CreateProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "CREATE NEW PROJECT" });

        public async Task WaitForDashboardAsync()
        {
            await _page.WaitForURLAsync("**/projects");
        }

        public async Task ClickCreateNewProjectAsync()
        {
            await CreateProjectButton.WaitForAsync();
            await CreateProjectButton.ClickAsync();
        }
    }
}
