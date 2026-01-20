using Microsoft.Playwright;
using WiseMigrateTests.Utils;

namespace WiseMigrateTests.Pages.ProjectCreation
{
    public class ProjectCreationPage
    {
        private readonly IPage _page;

        public ProjectCreationPage(IPage page)
        {
            _page = page;
        }

        // =========================
        // NAVIGATION (RESTORED)
        // =========================
        public async Task NavigateToProjectCreationPageAsync()
        {
            await _page.WaitForURLAsync("**/projects/create**", new()
            {
                Timeout = 60000
            });

            await Assertions.Expect(
                _page.GetByRole(AriaRole.Heading, new() { Name = "Create Project" })
            ).ToBeVisibleAsync();

            Logger.Info("Navigated to Project Creation page");
        }

        // =========================
        // LOCATORS
        // =========================
        private ILocator ProjectNameInput =>
            _page.GetByPlaceholder("Enter Project Name");

        private ILocator CustomerNameInput =>
            _page.GetByPlaceholder("Enter Customer Name");

        private ILocator DescriptionInput =>
            _page.GetByPlaceholder("Enter Description");

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
