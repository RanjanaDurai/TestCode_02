using Microsoft.Playwright;
using WiseMigrateTests.Pages.Project;

namespace WiseMigrateTests.Pages.ProjectCreation
{
    public class ProjectCreationPage
    {
        private readonly IPage _page;
        private readonly ProjectListPage _projectList;

        public ProjectCreationPage(IPage page)
        {
            _page = page;
            _projectList = new ProjectListPage(page);
        }

        
        public async Task NavigateToProjectCreationPageAsync()
        {
            await _projectList.WaitForDashboardAsync();
            await _projectList.ClickCreateNewProjectAsync();
        }
        public ILocator ProjectNameInput =>
            _page.Locator("input").Nth(0);

        public ILocator CustomerNameInput =>
            _page.Locator("input").Nth(1);

        public ILocator DescriptionTextArea =>
            _page.Locator("textarea");

        public ILocator CreateProjectButton =>
            _page.GetByRole(AriaRole.Button, new() { Name ="Create Project" });

        

        
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

        
  
    }
}
