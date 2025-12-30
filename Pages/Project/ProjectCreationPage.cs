using Microsoft.Playwright;

namespace WiseMigrateTests.Pages.Project
{
    public class ProjectCreationPage
    {
        private readonly IPage _page;

        public ProjectCreationPage(IPage page)
        {
            _page = page;
        }

        public ILocator ProjectNameInput =>
            _page.Locator("input").Nth(0);

        public ILocator CustomerNameInput =>
            _page.Locator("input").Nth(1);

        public ILocator DescriptionTextArea =>
            _page.Locator("textarea");

        public ILocator CreateProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Create Project" });

        public ILocator NextButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Next" });

        public ILocator CancelButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

        
        public async Task EnterProjectNameAsync(string name)
        {
            await ProjectNameInput.FillAsync(name);
        }

        public async Task EnterCustomerNameAsync(string customer)
        {
            await CustomerNameInput.FillAsync(customer);
        }

        public async Task EnterDescriptionAsync(string description)
        {
            await DescriptionTextArea.FillAsync(description);
        }

        public async Task ClickCreateProjectAsync()
        {
            await CreateProjectButton.ScrollIntoViewIfNeededAsync();
            await CreateProjectButton.ClickAsync();
        }

        public async Task ClickNextAsync()
        {
            await NextButton.ClickAsync();
        }

        public async Task ClickCancelAsync()
        {
            await CancelButton.ClickAsync();
        }
    }
}
