using Microsoft.Playwright;
using WiseMigrateTests.Pages.Project;

namespace WiseMigrateTests.PreRequisites
{
    public class BasicSetup
    {
        private readonly IPage _page;
        private readonly ProjectCreationPage _projectCreation;

        public BasicSetup(IPage page)
        {
            _page = page;
            _projectCreation = new ProjectCreationPage(page);
        }

        public string GenerateProjectName()
        {
            return $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
        }

        public async Task CreateProjectAsync(
            string projectName,
            string customerName,
            string description)
        {
            await _projectCreation.EnterProjectNameAsync(projectName);
            await _projectCreation.EnterCustomerNameAsync(customerName);
            await _projectCreation.EnterDescriptionAsync(description);

            await _projectCreation.ClickCreateProjectAsync();

            await _page.WaitForURLAsync("**/data-gathering**", new()
            {
                Timeout = 60000
            });
        }
    }
}
