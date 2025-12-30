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

        public async Task WaitForPageAsync()
        {
            await _page.WaitForSelectorAsync("text=Project Initialization");
        }

        public string GenerateProjectName()
        {
            return $"TC_Project_{DateTime.Now:yyyyMMdd_HHmmss}";
        }

        public async Task FillProjectDetails(string projectName, string customer, string description)
        {
            await _page.Locator("input").Nth(0).FillAsync(projectName);
            await _page.Locator("input").Nth(1).FillAsync(customer);
            await _page.Locator("textarea").FillAsync(description);
        }

        // ✅ SIMPLE & CORRECT CLICK
        public async Task ClickCreateProjectAsync()
        {
            var createBtn = _page.GetByRole(AriaRole.Button, new() { Name = "Create Project" });

            await createBtn.WaitForAsync();
            await createBtn.ScrollIntoViewIfNeededAsync();

            // normal click (no JS hacks)
            await createBtn.ClickAsync();

            // SPA navigation
            await _page.WaitForURLAsync("**/data-gathering**", new()
            {
                Timeout = 60000
            });
        }

        public async Task ClickNextAsync()
        {
            var nextBtn = _page.GetByRole(AriaRole.Button, new() { Name = "Next" });
            await nextBtn.ClickAsync();
        }

        public async Task WaitForUploadInfrastructurePageAsync()
        {
            await _page.WaitForSelectorAsync("text=Upload Infrastructure Data");
        }

        public async Task ClickCancelAsync()
        {
            var cancelBtn = _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            await cancelBtn.ClickAsync();
            await _page.WaitForURLAsync("**/projects");
        }
    }
}
