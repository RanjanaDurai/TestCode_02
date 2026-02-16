using Microsoft.Playwright;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Utils;
using System.Text.RegularExpressions;


namespace WiseMigrateTests.Pages.ProjectCreation
{
    public class ProjectCreationPage
    {
        private readonly IPage _page;

        private readonly ProjectListPage _projectList;

        public ProjectCreationPage(IPage page)
        {
            _page = page;
            _projectList= new ProjectListPage(page);
        }

        // NAVIGATION (RESTORED)
        public async Task NavigateToProjectCreationPageAsync()
        {
            await _projectList.WaitForDashboardToBeReadyAsync();
            await _projectList.ClickCreateNewProjectAsync();
            await _page.WaitForURLAsync("**/project-creation**", new()
            {
                Timeout = 60000
            });

            await Assertions.Expect(
                _page.GetByText("Project Initialization")
            ).ToBeVisibleAsync();

            Logger.Info("Navigated to Project Creation page");
        }

        // LOCATORS
        private ILocator ProjectNameInput =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "Project Name*" });

        private ILocator CustomerNameInput =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "Customer Name*" });

        private ILocator DescriptionInput =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "Description*" });

        private ILocator CreateProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "CREATE PROJECT" });

        private ILocator CancelButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "CANCEL" });

        // ACTIONS
        public async Task EnterProjectNameAsync(string projectName)
        {
            await ProjectNameInput.FillAsync(projectName);
        }
        public async Task EnterCustomerNameAsync(string customerName)
        {
            await CustomerNameInput.FillAsync(customerName);
        }
        public async Task EnterDescriptionAsync(string description)
        {
            await DescriptionInput.FillAsync(description);
        }
        public async Task ClickCreateProjectAsync()
        {
            await CreateProjectButton.ClickAsync();
        }
        public async Task ClickCancelAsync()
        {
            await CancelButton.ClickAsync();
        }
        private static readonly Random _random = new();
        public async Task RandomTargetCloudeProvider()
        {
            await _page.GetByText("Amazon Web Services").ClickAsync();
            var options = _page.Locator(".mud-list-item:not(.mud-disabled)");
            await options.First.WaitForAsync();
            int count = await options.CountAsync();
            await options.Nth(_random.Next(count)).ClickAsync();
        }
        public async Task ClickBackToDashBoardButton()
        {
            await _page.GetByText("Back to Dashboard").ClickAsync();
            await _page.GetByRole(AriaRole.Button).Filter(new() { HasTextRegex = new Regex("^$") }).Nth(3).ClickAsync();
        }
    }
}
