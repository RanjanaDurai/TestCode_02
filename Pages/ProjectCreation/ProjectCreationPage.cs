using Microsoft.Playwright;
using WiseMigrateTests.Pages.Project;
using WiseMigrateTests.Utils;

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

        // =========================
        // NAVIGATION (RESTORED)
        // =========================
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

        // =========================
        // LOCATORS
        // =========================
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

        // =========================
        // ACTIONS
        // =========================
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
    }
}
